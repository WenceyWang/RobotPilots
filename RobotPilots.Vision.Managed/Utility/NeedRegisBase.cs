using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Linq ;
using System . Reflection ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Utility
{

	public abstract class NeedRegisBase : IEquatable <NeedRegisBase>
	{

		[PublicAPI]
		public Guid Guid { get ; }

		protected static object Locker { get ; } = new object ( ) ;

		public ObjectType Type => TypeList . Single ( type => type . EntryType == GetType ( ) ) ;

		public static List <ObjectType> TypeList { get ; } = new List <ObjectType> ( ) ;


		protected NeedRegisBase ( ) { Guid = Guid . NewGuid ( ) ; }

		public bool Equals ( NeedRegisBase other )
		{
			if ( ReferenceEquals ( null , other ) )
			{
				return false ;
			}
			if ( ReferenceEquals ( this , other ) )
			{
				return true ;
			}

			return Guid . Equals ( other . Guid ) ;
		}

		public override string ToString ( ) { return $"{Type . Name} with {Guid}" ; }


		protected static NeedRegisBase Create ( ObjectType type , params object [ ] arguments )
		{
			#region Check Argument

			if ( type == null )
			{
				throw new ArgumentNullException ( nameof(type) ) ;
			}
			if ( ! TypeList . Contains ( type ) )
			{
				throw new ArgumentException ( $"{nameof(type)} have not being registered" , nameof(type) ) ;
			}

			#endregion

			NeedRegisBase instance = ( NeedRegisBase ) Activator . CreateInstance ( type . EntryType , arguments ) ;

			return instance ;
		}


		public static T ReadNecessaryValue <T> ( XElement element , string name )
		{
			if ( element == null )
			{
				throw new ArgumentNullException ( nameof(element) ) ;
			}
			if ( name == null )
			{
				throw new ArgumentNullException ( nameof(name) ) ;
			}

			string value = element . Attribute ( name ) ? . Value ;

			if ( value == null )
			{
				throw new ArgumentException (  /*string.Format(Resource.NecessaryValueNotFound, element, name)*/ ) ;
			}

			TypeConverter typeConverter = TypeDescriptor . GetConverter ( typeof ( T ) ) ;
			return ( T ) typeConverter . ConvertFromString ( value ) ;
		}

		public static T ReadUnnecessaryValue <T> ( XElement element , string name , T defaultValue )
		{
			if ( element == null )
			{
				throw new ArgumentNullException ( nameof(element) ) ;
			}
			if ( name == null )
			{
				throw new ArgumentNullException ( nameof(name) ) ;
			}

			string value = element . Attribute ( name ) ? . Value ;

			if ( value == null )
			{
				return defaultValue ;
			}

			TypeConverter typeConverter = TypeDescriptor . GetConverter ( typeof ( T ) ) ;
			return ( T ) typeConverter . ConvertFromString ( value ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
			{
				return false ;
			}
			if ( ReferenceEquals ( this , obj ) )
			{
				return true ;
			}
			if ( obj . GetType ( ) != GetType ( ) )
			{
				return false ;
			}

			return Equals ( ( NeedRegisBase ) obj ) ;
		}

		public override int GetHashCode ( ) { return Guid . GetHashCode ( ) ; }

		public static bool operator == ( NeedRegisBase left , NeedRegisBase right ) { return Equals ( left , right ) ; }

		public static bool operator != ( NeedRegisBase left , NeedRegisBase right ) { return ! Equals ( left , right ) ; }

		[PublicAPI]
		protected static void RegisType ( ObjectType subType )
		{
			lock ( Locker )
			{
				if ( subType == null )
				{
					throw new ArgumentNullException ( nameof(subType) ) ;
				}

				if ( TypeList . Contains ( subType ) )
				{
					throw new InvalidOperationException ( "This type have been registed" ) ;
				}

				if ( TypeList . Any ( type => type . Name == subType . Name ) )
				{
					throw new Exception ( "Name is Invilable" ) ; //Todo:Resources
				}


				TypeList . Add ( subType ) ;
			}
		}

		[AttributeUsage ( AttributeTargets . Class )]
		public abstract class NeedRegisAttributeBase : Attribute
		{

		}

	}


	public abstract class NeedRegisBase <TType , TAttribute , T> : NeedRegisBase
		where TType : RegisType <TType , TAttribute , T>
		where TAttribute : NeedRegisBase . NeedRegisAttributeBase
		where T : NeedRegisBase <TType , TAttribute , T>
	{

		public new TType Type => base . Type as TType ;

		public new static IEnumerable <TType> TypeList => NeedRegisBase . TypeList . OfType <TType> ( ) ;

		public static TType FindType ( string name ) { return TypeList . Single ( type => type . Name == name ) ; }

		protected static T Create ( TType type , params object [ ] arguments )
		{
			#region Check Argument

			if ( type == null )
			{
				throw new ArgumentNullException ( nameof(type) ) ;
			}
			if ( ! TypeList . Contains ( type ) )
			{
				throw new ArgumentException ( $"{nameof(type)} have not being registered" , nameof(type) ) ;
			}

			#endregion

			T instance = ( T ) NeedRegisBase . Create ( type , arguments ) ;

			return instance ;
		}

		protected static void RegisType <TSub> ( TSub subType ) where TSub : TType
		{
			lock ( Locker )
			{
				NeedRegisBase . RegisType ( subType ) ;
			}
		}

		[PublicAPI]
		protected static TType RegisType ( Type entryType )
		{
			lock ( Locker )
			{
				#region Check Argument

				if ( entryType == null )
				{
					throw new ArgumentNullException ( nameof(entryType) ) ;
				}

				if ( ! typeof ( T ) . GetTypeInfo ( ) . IsAssignableFrom ( entryType . GetTypeInfo ( ) ) )
				{
					throw new ArgumentException ( $"{nameof(entryType)} should assignable from {nameof(T)}" , nameof(entryType) ) ;
				}

				if ( entryType . GetTypeInfo ( ) . GetCustomAttributes ( typeof ( TAttribute ) , false ) . Single ( ) == null )
				{
					throw new ArgumentException ( $"{nameof(entryType)} should have atribute {nameof(TAttribute)}" ,
												nameof(entryType) ) ;
				}

				if ( TypeList . Any ( type => type . EntryType == entryType ) )
				{
					throw new InvalidOperationException ( $"{nameof(entryType)} have regised" ) ;
				}

				#endregion

				TType instance = ( TType ) Activator . CreateInstance ( typeof ( TType ) , entryType ) ;

				NeedRegisBase . RegisType ( instance ) ;

				return instance ;
			}
		}

		protected static bool Loaded { get; set; }

		//Todo:
		public static void LoadAll ( )
		{

			lock (Locker)
			{
				if (Loaded)
				{
					return;
				}

				//Todo:Load All internal type
				foreach (TypeInfo type in typeof(T).GetTypeInfo().
															Assembly.DefinedTypes.
															Where(type => type.GetCustomAttributes(typeof(TAttribute), false).Any()
																		&& typeof(T).GetTypeInfo().IsAssignableFrom(type)))
				{
					RegisType(type.AsType()); //Todo:resources?
				}

				Loaded = true;
			}

		}

	}

}
