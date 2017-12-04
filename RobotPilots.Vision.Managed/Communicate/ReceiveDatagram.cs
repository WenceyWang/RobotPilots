using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public abstract class ReceiveDatagram : Datagram
	{

		public XElement Source { get ; }

		protected ReceiveDatagram ( XElement source ) { Source = source ; }

		public sealed override XElement ToXElement ( ) { return Source ; }

	}

}
