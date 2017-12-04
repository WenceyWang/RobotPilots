using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public abstract class Datagram : NeedRegisBase <DatagramType , DatagramAttribute , Datagram> , ISelfSerializeable
	{

		public abstract XElement ToXElement ( ) ;

		public static Datagram Parse ( XElement element )
		{
			return Create ( TypeList . Single ( type => type . XmlName == element . Name ) , element ) ;
		}

		public static Datagram Parse ( byte [ ] data )
		{
			return Create ( TypeList . Single ( type => ( ( byte ) type . BinaryType ) == data [ 1 ] ) ) ;
		}

		public abstract byte [ ] ToBinary ( ) ;

		public override string ToString ( ) { return ToXElement ( ) . ToString ( ) ; }

		[Startup]
		public static void LoadDatagram ( )
		{
		}

	}

}
