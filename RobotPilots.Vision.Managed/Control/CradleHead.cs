using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Math ;

namespace RobotPilots . Vision . Managed . Control
{

	[PublicAPI]
	public abstract class CradleHead
	{

		public AnglePosition Position { get ; }

		public AnglePosition MoveTarget { get ; set ; }

		public abstract TimeSpan ExpectedMoveTime ( AnglePosition target ) ;

		//public 

	}

}
