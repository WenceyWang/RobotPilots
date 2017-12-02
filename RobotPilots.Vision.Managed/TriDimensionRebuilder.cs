using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed
{

	public abstract class TriDimensionRebuilder : NeedRegisBase <TriDimensionRebuilderType ,
		TriDimensionRebuilderAttreibute , TriDimensionRebuilder>
	{

		public abstract Point3f Transform ( Point2f imagePoint , CameraPosition cameraPosition ) ;

	}

	//public abstract class

	public class TriDimensionRebuilderAttreibute : NeedRegisAttributeBase
	{

	}

	public class TriDimensionRebuilderType : RegisType <TriDimensionRebuilderType , TriDimensionRebuilderAttreibute ,
		TriDimensionRebuilder>
	{

		public TriDimensionRebuilderType ( [NotNull] Type entryType ) : base ( entryType ) { }

	}

}
