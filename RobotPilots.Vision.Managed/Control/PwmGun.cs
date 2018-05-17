using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Control
{

	public class PwmGun : Gun
	{

		public LinearInterpolationLookupTable DutyRatioSpeedTable { get ; }

		public short DutyRatio { get; set; }

		public PwmGun(byte id, GunSize size) : base(id, size)
		{
			DutyRatioSpeedTable = new LinearInterpolationLookupTable ( ) ;
		}

		public PwmGun(XElement element) : base(element) { DutyRatioSpeedTable = new LinearInterpolationLookupTable ( ) ; }

		public override float BulletSpeed { get; set; }

	}

}