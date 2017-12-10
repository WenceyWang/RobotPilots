using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public abstract class ReceiveDatagram : Datagram
	{

		public XElement XmlSource { get ; }

		public byte [ ] BinarySource { get ; }

		protected ReceiveDatagram ( XElement xmlSource ) { XmlSource = xmlSource ; }

		protected ReceiveDatagram ( byte [ ] binarySource ) { BinarySource = binarySource ; }

		public sealed override XElement ToXElement ( ) { return XmlSource ; }

	}

}
