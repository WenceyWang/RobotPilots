using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Control
{

	public class ClosedloopGun : Gun
	{
		public float WheelRadius { get; }

		public float RotatingSpeed { get; set ; }

		public ClosedloopGun(byte id, GunSize size , float wheelRadius ) : base(id, size)
		{
			WheelRadius = wheelRadius ;
		}

		public ClosedloopGun(XElement element , float wheelRadius ) : base(element) { WheelRadius = wheelRadius ; }

		public override float BulletSpeed { get; set; }

	}

}