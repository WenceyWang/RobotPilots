using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed
{

	public class ObjectType
	{

		/// <summary>
		///     Guid of this Type
		///     Always return EntryType.Guid
		/// </summary>
		public Guid Guid => EntryType . GetTypeInfo ( ) . GUID ;

		public Type EntryType { get ; }

		public string Name => EntryType . FullName ;

		public static List <ObjectType> TypeList => NeedRegisBase . TypeList ;

		public ObjectType ( [NotNull] Type entryType )
		{
			EntryType = entryType ?? throw new ArgumentNullException ( nameof(entryType) ) ;
		}

	}

}
