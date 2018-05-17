using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate . Environment
{

	[PublicAPI]
	[Datagram(nameof(BinaryDatagramType.UpdateSupplyStationStatus), BinaryDatagramType.UpdateSupplyStationStatus)]
	public class UpdateSupplyStationStatusDatagram : ReceiveDatagram
	{

		public UpdateSupplyStationStatusDatagram ( XElement xmlSource ) : base ( xmlSource )
		{
			SmallBulletAmount = ReadNecessaryValue <short> ( xmlSource , nameof(SmallBulletAmount) ) ;
			LargeBulletAmount = ReadNecessaryValue <short> ( xmlSource , nameof(LargeBulletAmount) );
		}

		public UpdateSupplyStationStatusDatagram ( byte [ ] binarySource ) : base ( binarySource )
		{
			SmallBulletAmount = BitConverter . ToInt16 ( binarySource , 0 ) ;
			LargeBulletAmount = BitConverter . ToInt16 ( binarySource , 2 );
		}

		public short SmallBulletAmount { get; set; }

		public short LargeBulletAmount { get; set; }


	}

}