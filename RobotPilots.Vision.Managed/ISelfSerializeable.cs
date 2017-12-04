using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed
{

	public interface ISelfSerializeable
	{

		/// <summary>
		/// </summary>
		/// <returns></returns>
		[NotNull]
		XElement ToXElement ( ) ;

	}

}
