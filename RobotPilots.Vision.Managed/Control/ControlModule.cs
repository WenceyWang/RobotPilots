using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using RobotPilots . Vision . Managed . Communicate ;
using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Control
{

	[Module]
	public class ControlModule : IModule
	{

		public CradleHead CradleHead { get ; set ; }

		public Gun Gun { get ; set ; }

		public static ControlModule Current { get ; set ; }

		public string [ ] Dependencies { get ; } = { nameof(CommunicateModule) } ;

		public void Prepare ( Configurations configuration )
		{
			Current = this ;

			CradleHead = new CradleHead ( ) ;
			Gun = new Gun ( ) ;
		}

		public void Dispose ( ) { }

	}

}
