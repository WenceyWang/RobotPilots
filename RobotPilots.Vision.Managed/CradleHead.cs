using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed
{

	public abstract class CradleHead
	{

		public AnglePosition Position { get ; }

		public AnglePosition MoveTarget { get ; set ; }

		public abstract TimeSpan ExpectedMoveTime ( AnglePosition target ) ;

	}

}
