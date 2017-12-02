using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed
{

	/// <summary>
	///     Point out any method should be called before using this lib
	/// </summary>
	[AttributeUsage ( AttributeTargets . Method , Inherited = false )]
	public sealed class StartupAttribute : Attribute
	{

	}

}
