using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Utility ;
using RobotPilots . Vision . Managed . Visual ;

namespace RobotPilots . Vision . Managed
{

	public abstract class TriDimensionRebuilder : NeedRegisBase <TriDimensionRebuilder . TriDimensionRebuilderType ,
		TriDimensionRebuilder . TriDimensionRebuilderAttribute , TriDimensionRebuilder>
	{

		public abstract Point3f Transform ( Point2f imagePoint , CameraPosition cameraPosition ) ;

		public class TriDimensionRebuilderAttribute : NeedRegisAttributeBase
		{

		}

		public class TriDimensionRebuilderType : RegisType <TriDimensionRebuilderType , TriDimensionRebuilderAttribute ,
			TriDimensionRebuilder>
		{

			public TriDimensionRebuilderType ( [NotNull] Type entryType ) : base ( entryType ) { }

		}

	}

	[TriDimensionRebuilder]
	public class BinocularTriDimensionRebuilder : TriDimensionRebuilder
	{

		public BinocularCamera Camera { get ; set ; }

		public override Point3f Transform ( Point2f imagePoint , CameraPosition cameraPosition )
		{
			//Camera.
			return default ( Point3f ) ;
		}

	}

}
