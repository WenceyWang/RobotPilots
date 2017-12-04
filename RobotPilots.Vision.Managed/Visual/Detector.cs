using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual
{

	public abstract class Detector : NeedRegisBase <Detector . DetectorType , Detector . DetectorAttribute , Detector>
	{

		private static bool Loaded { get ; set ; }

		public abstract List <Point2f> Detcet ( Mat frame ) ;

		[Startup]
		public void LoadDetector ( )
		{
			lock ( Locker )
			{
				if ( Loaded )
				{
					return ;
				}

				foreach ( TypeInfo type in typeof ( Application ) . GetTypeInfo ( ) .
																	Assembly . DefinedTypes .
																	Where ( type => type . GetCustomAttributes ( typeof ( DetectorAttribute ) , false ) . Any ( )
																					&& typeof ( NeedRegisBase ) . GetTypeInfo ( ) . IsAssignableFrom ( type ) ) )
				{
					RegisType ( new DetectorType ( type . AsType ( ) ) ) ; //Todo:resources?
				}

				Loaded = true ;
			}
		}

		public class DetectorAttribute : NeedRegisAttributeBase
		{

		}

		public class DetectorType : RegisType <DetectorType , DetectorAttribute , Detector>
		{

			public DetectorType ( [NotNull] Type entryType ) : base ( entryType ) { }

		}

	}

}
