using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Communicate ;
using RobotPilots . Vision . Managed . Math ;

namespace RobotPilots . Vision . Managed . Control
{

	[PublicAPI]
	public abstract class CradleHead
	{

		public AnglePosition Position { get ; private set ; }

		public AnglePosition MoveTarget { get ; set ; }

		public abstract TimeSpan ExpectedMoveTime ( AnglePosition target ) ;

		//Todo:??
		public void ProcessPackage ( object caller , ReceiveDatagramEventArgs args )
		{
			if ( args . Datagram is CradleHeadPositionDatagram datagram )
			{
				Position = datagram . Position ;
			}
		}

	}

}
