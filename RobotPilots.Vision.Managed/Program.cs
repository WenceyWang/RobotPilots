using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed
{

	public class Program
	{

		public static Program Current { get ; private set ; }


		public void Run ( )
		{
			List <VideoCapture> videoCaptures = new List <VideoCapture> ( ) ;

			for ( int device = 0 ; device < 10 ; device++ )
			{
				VideoCapture cap = new VideoCapture ( device ) ;
				if ( cap . IsOpened ( ) )
				{
					videoCaptures . Add ( cap ) ;
				}
			}


			CvCamera leftCamera = new CvCamera ( videoCaptures . First ( ) ) ;
			CvCamera rightCamera = new CvCamera ( videoCaptures . Last ( ) ) ;

			using ( Window leftWindow = new Window ( "Left" ) )
			{
				using ( Window rightWindow = new Window ( "Right" ) )
				{
					while ( true )
					{
						using ( Mat leftImage = leftCamera . Read ( ) )
						{
							using ( Mat rightImage = rightCamera . Read ( ) )
							{
								leftWindow . ShowImage ( leftImage ) ;

								rightWindow . ShowImage ( rightImage ) ;

								Cv2 . WaitKey ( 20 ) ;
							}
						}
					}

					//GC . Collect ( 2 , GCCollectionMode . Forced , true ) ;
				}

				//BinocularCamera camera = new BinocularCamera(leftCamera, rightCamera);

				//camera.Open(0, 1,new Size(640, 480));

				//List < Tuple < String ^, String ^>^>^ files = gcnew List < Tuple < String ^, String ^>^> ();

				//vector<tuple<cv::Mat, cv::Mat>> images;
				//for ( int i = 1 ; i < 15 ; i++ )
				//{

				//}
			}
		}


		public static void Main ( string [ ] args )
		{
			Startup . RunAllTask ( ) . Wait ( ) ;
			try
			{
				Current = new Program ( ) ;

				Current . Run ( ) ;
			}
			catch ( Exception e )
			{
				Console . WriteLine ( e ) ;
			}
		}

	}

}
