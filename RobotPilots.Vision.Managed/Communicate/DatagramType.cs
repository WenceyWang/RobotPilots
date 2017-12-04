using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate
{

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
