using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using RobotPilots . Vision . Managed . Communicate ;

namespace RobotPilots . Vision . Managed . Control
{

	public class Gun
	{

		public byte Id { get ; set ; }

		public GunType Type { get ; set ; }

		public float FrictionSpeed { get ; set ; }

		public void Fire ( byte amount )
		{
			CommunicateManager . Current . SendDatagram ( new FireDatagram ( Id , amount ) ) ;
		}

	}

}
