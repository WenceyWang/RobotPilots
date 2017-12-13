using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using RJCP . IO . Ports ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[Module]
	public class CommunicateModule : IModule
	{

		public static CommunicateModule Current { get ; set ; }

		public CommunicateManager Manager { get ; set ; }


		public string [ ] Dependencies { get ; } = { } ;

		public void Prepare ( Configurations configuration )
		{
			Current = this ;

			SerialPortStream stream = new SerialPortStream ( configuration . SerialPortName ,
															configuration . SerialPortBaudRate ,
															configuration . SerialPortDataBits ,
															configuration . SerialPortParity ,
															configuration . SerialPortStopBits ) ;

			stream . Open ( ) ;

			Manager = new CommunicateManager ( stream , configuration . ReceiveMode , configuration . SendMode ) ;

			Manager . Run ( ) ;
		}

		public void Dispose ( )
		{
			Manager ? . Stop ( ) ;
			Manager ? . UnderlyingStream ? . Close ( ) ;
		}

	}

}
