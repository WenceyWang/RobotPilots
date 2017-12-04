using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public class ReceivePackageEventArgs : EventArgs
	{

		public ReceiveDatagram Datagram { get ; set ; }

		public ReceivePackageEventArgs ( ReceiveDatagram datagram ) { Datagram = datagram ; }

	}

}
