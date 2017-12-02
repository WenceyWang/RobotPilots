using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed
{

	public struct AnglePosition
	{

		public AnglePosition ( Angle xPitch , Angle yPitch )
		{
			XPitch = xPitch ;
			YPitch = yPitch ;
		}

		public Angle XPitch { get ; }

		public Angle YPitch { get ; }

	}

}
