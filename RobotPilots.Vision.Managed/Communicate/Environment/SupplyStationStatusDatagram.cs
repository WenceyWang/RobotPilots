using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate . Environment
{

	[PublicAPI]
	[Datagram(nameof(BinaryDatagramType.SupplyStationStatus), BinaryDatagramType.SupplyStationStatus)]
	public class SupplyStationStatusDatagram : SendDatagram
	{

		public short SmallBulletAmount { get; set; }

		public short LargeBulletAmount { get; set; }

		public SupplyStationStatusDatagram ( short smallBulletAmount , short largeBulletAmount )
		{
			SmallBulletAmount = smallBulletAmount ;
			LargeBulletAmount = largeBulletAmount ;
		}

		public override XElement ToXElement()
		{
			XElement result = base.ToXElement();

			result.SetAttributeValue(nameof(SmallBulletAmount), SmallBulletAmount);
			result.SetAttributeValue(nameof(LargeBulletAmount), LargeBulletAmount);

			return result;
		}

		public override byte[] ToBinary()
		{
			List<byte> byties = new List<byte>(1);

			byties.AddRange(BitConverter.GetBytes(SmallBulletAmount));

			byties.AddRange(BitConverter.GetBytes(LargeBulletAmount));

			return byties.ToArray();
		}

	}

}