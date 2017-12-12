using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Utility
{

	public interface IModule : IDisposable
	{

		string [ ] Dependencies { get ; }

		void Prepare ( Configurations configuration ) ;

		void Dispose ( ) ;

	}

}
