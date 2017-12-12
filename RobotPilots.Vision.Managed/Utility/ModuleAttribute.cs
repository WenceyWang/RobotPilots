using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Utility
{

	/// <summary>
	///     Point out any method should be called before start
	/// </summary>
	[AttributeUsage ( AttributeTargets . Class , Inherited = false )]
	public sealed class ModuleAttribute : Attribute
	{

	}

}
