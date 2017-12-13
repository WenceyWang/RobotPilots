using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Communicate ;
using RobotPilots . Vision . Managed . Math ;

namespace RobotPilots . Vision . Managed . Control
{

	[PublicAPI]
	public class CradleHead
	{

		public AnglePosition Position { get ; private set ; }

		public AnglePosition MoveTarget { get ; private set ; }

		public void SetNewTarget ( AnglePosition target )
		{
			CommunicateModule . Current . Manager . SendDatagram ( new TargetAngleDatagram ( target ) ) ;
		}

		public void SetNewTarget ( Point3f target )
		{
			CommunicateModule . Current . Manager . SendDatagram ( new TargetPositionDatagram ( target ) ) ;
		}

		public void SetDeltaAngle ( AnglePosition targetDelta )
		{
			CommunicateModule . Current . Manager . SendDatagram ( new TargetDeltaAngleDatagram ( targetDelta ) ) ;
		}

		public TimeSpan ExpectedMoveTime ( AnglePosition target ) { return TimeSpan . Zero ; }

		//Todo:??
		public void ProcessPackage ( object caller , ReceiveDatagramEventArgs args )
		{
			switch ( args . Datagram )
			{
				case CradleHeadPositionDatagram cradleHeadPositionDatagram :
				{
					Position = cradleHeadPositionDatagram . Position ;
					break ;
				}
				case CradleHeadTargetDatagram cradleHeadTargetDatagram :
				{
					MoveTarget = cradleHeadTargetDatagram . Target ;
					break ;
				}
			}
		}

	}

}
