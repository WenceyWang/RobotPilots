using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . IO ;
using System . Linq ;
using System . Threading ;

using Microsoft . Extensions . Logging ;

using RobotPilots . Vision . Managed . Communicate ;
using RobotPilots . Vision . Managed . Math ;
using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed
{

	public class Application
	{

		private static ILogger Logger { get ; set ; }

		internal static ILoggerFactory LoggerFactory { get ; private set ; }

		public static Configurations Configuration { get ; set ; }

		public static Application Current { get ; private set ; }

		public Application ( ) { Current = this ; }

		[Startup]
		public static void StartUp ( )
		{
			LoggerFactory = new LoggerFactory ( ) . AddConsole ( ) . AddDebug ( ) ;
			Logger = LoggerFactory . CreateLogger ( typeof ( Application ) ) ;

			#region Loading Setting

			if ( File . Exists ( FileNameConst . SettingFile ) )
			{
				FileStream settingFile = File . OpenRead ( FileNameConst . SettingFile ) ;
				Logger . LogInformation ( "Setting file found, load it." ) ;
				Configuration = Configurations . Load ( settingFile ) ;
			}
			else
			{
				Logger . LogInformation ( "Setting file not found, will generate it." ) ;
				Configuration = Configurations . GenerateNew ( ) ;

				string config = Configuration . Save ( ) ;
				FileStream settingFile = File . OpenWrite ( FileNameConst . SettingFile ) ;
				StreamWriter writer = new StreamWriter ( settingFile ) ;
				writer . Write ( config ) ;
				writer . Dispose ( ) ;
			}

			#endregion
		}

		public void Run ( )
		{
			IEnumerable <Type> moduleTypes = typeof ( Application ) . Assembly . GetTypes ( ) .
																	Where ( type => type . GetCustomAttributes ( typeof ( ModuleAttribute ) , false ) . Any ( ) ) ;

			List <IModule> modulesToPrepare =
				moduleTypes . Select ( type => ( IModule ) Activator . CreateInstance ( type ) ) . ToList ( ) ;

			List <IModule> preparedModules = new List <IModule> ( modulesToPrepare . Count ) ;

			int currentCircleLoadModCount ;

			do
			{
				currentCircleLoadModCount = 0 ;

				List <IModule> canPrepareModules = modulesToPrepare . Where ( mod => mod . Dependencies . All ( dependency
																													=> preparedModules . Any ( loadedMod =>
																																					loadedMod . GetType ( ) . Name == dependency ) ) ) . ToList ( ) ;
				foreach ( IModule mod in canPrepareModules )
				{
					currentCircleLoadModCount++ ;
					modulesToPrepare . Remove ( mod ) ;

					mod . Prepare ( Configuration ) ;
					Logger . LogInformation ( $"Module {mod . GetType ( ) . Name} Prepared" ) ;

					preparedModules . Add ( mod ) ;
				}
			}
			while ( currentCircleLoadModCount != 0 ) ;

			if ( modulesToPrepare . Any ( ) )
			{
				throw new Exception ( "Some modules failed to load" , null ) ;
			}


			DateTime startTime = DateTime . Now ;

			while ( true )
			{
				Thread . Sleep ( 30 ) ;

				TargetAngleDatagram target = new TargetAngleDatagram ( new AnglePosition (
																			System . Math . Sin ( Angle . FromDegree ( ( DateTime . Now - startTime ) . TotalSeconds * 90 ) .
																										Radius ) * 60 ,
																			0 ) ) ;

				CommunicateModule . Current . Manager . SendDatagram ( target ) ;
			}


			//List <VideoCapture> videoCaptures = new List <VideoCapture> ( ) ;

			//for ( int device = 0 ; device < 10 ; device++ )
			//{
			//	VideoCapture cap = new VideoCapture ( device ) ;
			//	if ( cap . IsOpened ( ) )
			//	{
			//		videoCaptures . Add ( cap ) ;
			//	}
			//}


			//CvCamera leftCamera = new CvCamera ( videoCaptures . First ( ) ) ;
			//CvCamera rightCamera = new CvCamera ( videoCaptures . Last ( ) ) ;

			//using ( Window leftWindow = new Window ( "Left" ) )
			//{
			//	using ( Window rightWindow = new Window ( "Right" ) )
			//	{
			//		while ( true )
			//		{
			//			using ( Mat leftImage = leftCamera . Read ( ) )
			//			{
			//				using ( Mat rightImage = rightCamera . Read ( ) )
			//				{
			//					leftWindow . ShowImage ( leftImage ) ;

			//					rightWindow . ShowImage ( rightImage ) ;

			//					Cv2 . WaitKey ( 20 ) ;
			//				}
			//			}
			//		}

			//		//GC . Collect ( 2 , GCCollectionMode . Forced , true ) ;
			//	}

			//	//CvBinocularCamera camera = new CvBinocularCamera(leftCamera, rightCamera);

			//	//camera.Open(0, 1,new Size(640, 480));

			//	//List < Tuple < String ^, String ^>^>^ files = gcnew List < Tuple < String ^, String ^>^> ();

			//	//vector<tuple<cv::Mat, cv::Mat>> images;
			//	//for ( int i = 1 ; i < 15 ; i++ )
			//	//{

			//	//}
			//}
		}

	}

}
