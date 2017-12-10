using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Math
{

	public struct AnglePosition
	{

		public AnglePosition ( Angle xYaw , Angle yPitch )
		{
			XYaw = xYaw ;
			YPitch = yPitch ;
		}

		public Angle XYaw { get ; }

		public Angle YPitch { get ; }

	}

}
