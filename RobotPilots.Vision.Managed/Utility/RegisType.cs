using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Utility
{

	public abstract class RegisType <TType , TAttribute , T> : ObjectType
		where T : NeedRegisBase <TType , TAttribute , T>
		where TType : RegisType <TType , TAttribute , T>
		where TAttribute : NeedRegisBase . NeedRegisAttributeBase
	{

		public RegisType ( [NotNull] Type entryType ) : base ( entryType )
		{
			if ( ! typeof ( T ) . GetTypeInfo ( ) . IsAssignableFrom ( entryType . GetTypeInfo ( ) ) )
			{
				throw new ArgumentException ( $"{nameof(entryType)} is not {nameof(T)}" ) ;
			}
		}

	}

}
