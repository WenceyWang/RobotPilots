using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using RobotPilots . Vision . Managed . Communicate ;
using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Control
{

	[Module]
	public class ControlModule : IModule
	{

		public CradleHead CradleHead { get ; set ; }

		public List <Gun> Guns = new List <Gun> ( ) ;

		public static ControlModule Current { get ; set ; }

		public string [ ] Dependencies { get ; } = { nameof(CommunicateModule) } ;

		public void Prepare ( Configurations configuration )
		{
			Current = this ;

			CradleHead = new CradleHead ( ) ;
			Guns = new List<Gun>() ;

			XElement guns = XElement . Parse ( configuration . GunConfig ) ;

			foreach ( XElement gunElement in guns.Elements() )
			{
				Gun gun = Gun.Parse(gunElement) ;
				Guns . Add ( gun ) ;
			}



		}

		public void Dispose ( ) { }

	}

}
