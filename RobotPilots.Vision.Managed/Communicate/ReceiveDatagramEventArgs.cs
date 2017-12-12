using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public class ReceiveDatagramEventArgs : EventArgs
	{

		public ReceiveDatagram Datagram { get ; set ; }

		public ReceiveDatagramEventArgs ( ReceiveDatagram datagram ) { Datagram = datagram ; }

	}

}
