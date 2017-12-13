using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[PublicAPI]
	[Datagram ( nameof(BinaryDatagramType . Fire) , BinaryDatagramType . Fire )]
	public class FireDatagram : SendDatagram
	{

		public byte Amount { get ; set ; }

		public byte GunId { get ; set ; }

		public FireDatagram ( byte gunId , byte amount )
		{
			Amount = amount ;
			GunId = gunId ;
		}

		public override XElement ToXElement ( )
		{
			XElement result = base . ToXElement ( ) ;

			result . SetAttributeValue ( nameof(Amount) , Amount ) ;

			return result ;
		}

		public override byte [ ] ToBinary ( )
		{
			List <byte> byties = new List <byte> ( 1 ) ;

			byties . AddRange ( BitConverter . GetBytes ( GunId ) ) ;

			byties . AddRange ( BitConverter . GetBytes ( Amount ) ) ;

			return byties . ToArray ( ) ;
		}

	}

}
