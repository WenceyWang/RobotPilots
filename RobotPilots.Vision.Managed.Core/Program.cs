using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Core
{

	public class Program
	{

		public static void Main ( string [ ] args )
		{
			Startup . RunAllTask ( ) . Wait ( ) ;
			try
			{
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
