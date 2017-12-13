using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Utility
{

	/// <summary>
	///     Point out any module should be prepare
	/// </summary>
	[AttributeUsage ( AttributeTargets . Class , Inherited = false )]
	public sealed class ModuleAttribute : Attribute
	{

	}

}
