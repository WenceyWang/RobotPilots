using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public abstract class Datagram : NeedRegisBase <Datagram . DatagramType , Datagram . DatagramAttribute , Datagram> ,
									ISelfSerializeable
	{

		public abstract XElement ToXElement ( ) ;

		public static Datagram Parse ( XElement element )
		{
			return Create ( TypeList . Single ( type => type . XmlName == element . Name ) , element ) ;
		}

		public static Datagram Parse ( BinaryDatagramType type , byte [ ] data )
		{
			return Create ( TypeList . Single ( typ => typ . BinaryType == type ) , data ) ;
		}

		public abstract byte [ ] ToBinary ( ) ;

		public override string ToString ( ) { return ToXElement ( ) . ToString ( ) ; }

		[Startup]
		public static void LoadDatagram ( )
		{
		}

		public class DatagramAttribute : NeedRegisAttributeBase
		{

			public string XmlName { get ; }

			public BinaryDatagramType BinaryType { get ; }

			public DatagramAttribute ( string xmlName , BinaryDatagramType binaryType )
			{
				XmlName = xmlName ;
				BinaryType = binaryType ;
			}

		}

		public class DatagramType : RegisType <DatagramType , DatagramAttribute , Datagram>
		{

			public string XmlName { get ; }

			public BinaryDatagramType BinaryType { get ; }


			public DatagramType ( [NotNull] Type entryType ) : base ( entryType )
			{
				DatagramAttribute attribute =
					( DatagramAttribute ) entryType . GetCustomAttributes ( typeof ( DatagramAttribute ) , false ) .
													Single ( ) ;
				XmlName = attribute . XmlName ;
				BinaryType = attribute . BinaryType ;
			}

		}

	}

}
