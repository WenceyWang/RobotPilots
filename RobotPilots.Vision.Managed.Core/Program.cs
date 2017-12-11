using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Core
{

	public class Program
	{

		public static void Main ( string [ ] args )
		{
			try
			{
				Startup . RunAllTask ( ) . Wait ( ) ;

				Application application = new Application ( ) ;

				application . Run ( ) ;
			}
			catch ( Exception e )
			{
				Console . WriteLine ( e ) ;
			}
		}

	}

}
