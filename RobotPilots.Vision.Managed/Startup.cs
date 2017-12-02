using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;
using System . Threading . Tasks ;

namespace RobotPilots . Vision . Managed
{

	public static class Startup
	{

		public static Task RunAllTask ( )
		{
			List <Task> tasks = new List <Task> ( ) ;
			foreach ( TypeInfo type in typeof ( Program ) . GetTypeInfo ( ) . Assembly . DefinedTypes )
			{
				foreach ( MethodInfo method in type . DeclaredMethods )
				{
					if ( method . GetCustomAttributes ( typeof ( StartupAttribute ) ) . Any ( ) )
					{
						tasks . Add ( Task . Run ( ( ) => method . Invoke ( null , new object [ ] { } ) ) ) ;
					}
				}
			}

			return Task . WhenAll ( tasks ) ;
		}

	}

}
