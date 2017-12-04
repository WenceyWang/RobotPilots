using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

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

}
