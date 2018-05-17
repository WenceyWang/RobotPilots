using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate . Gun
{

	[PublicAPI]
	[Datagram ( nameof(BinaryDatagramType . FrictionSpeed) , BinaryDatagramType . FrictionSpeed )]
	public class FrictionSpeedDatagram : SendDatagram
	{

		public byte GunId { get ; set ; }

		public float Frequency { get ; set ; }


		public FrictionSpeedDatagram ( byte gunId , float frequency )
		{
			GunId = gunId ;
			Frequency = frequency ;
		}

		public override XElement ToXElement ( )
		{
			XElement result = base . ToXElement ( ) ;

			result . SetAttributeValue ( nameof(Frequency) , Frequency ) ;

			return result ;
		}

		public override byte [ ] ToBinary ( )
		{
			List <byte> byties = new List <byte> ( 5 ) ;

			byties . AddRange ( BitConverter . GetBytes ( GunId ) ) ;

			byties . AddRange ( BitConverter . GetBytes ( Frequency ) ) ;

			return byties . ToArray ( ) ;
		}

	}

}
