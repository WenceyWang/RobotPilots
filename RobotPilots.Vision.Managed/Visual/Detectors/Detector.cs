using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Visual . Detectors
{

	public abstract class Detector : NeedRegisBase <Detector . DetectorType , Detector . DetectorAttribute , Detector>
	{

		public abstract List <Point2f> Detcet ( Mat frame ) ;

		[Startup]
		public static void LoadDetector ( )
		{
			LoadAll ( ) ;
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
