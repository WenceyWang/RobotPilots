using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . IO ;
using System . Linq ;
using System . Reflection ;
using System . Text ;

using JetBrains . Annotations ;

using RJCP . IO . Ports ;

using RobotPilots . Vision . Managed . Communicate ;

namespace RobotPilots . Vision . Managed . Utility
{

	[PublicAPI]
	public class Configurations
	{

		public enum ConfigurationCategory
		{

			General = 0 ,

			Communicate = 1 ,

			Strategy = 2

		}

		[ConfigurationItem ( ConfigurationCategory . General ,
			nameof(DebugConsoleOutput) ,
			"" ,
#if DEBUG
			true
#else
			false
#endif
		)]
		public bool DebugConsoleOutput { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SerialPortName) , "" , "COM1" )]
		public string SerialPortName { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SerialPortBaudRate) , "" , 9600 )]
		public int SerialPortBaudRate { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SerialPortParity) , "" , Parity . None )]
		public Parity SerialPortParity { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SerialPortDataBits) , "" , 8 )]
		public int SerialPortDataBits { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SerialPortStopBits) , "" , StopBits . One )]
		public StopBits SerialPortStopBits { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(SendMode) , "" , SerializationMode . Binary )]
		public SerializationMode SendMode { get ; set ; }

		[ConfigurationItem ( ConfigurationCategory . Communicate , nameof(ReceiveMode) , "" , SerializationMode . Binary )]
		public SerializationMode ReceiveMode { get ; set ; }

		public string Save ( )
		{
			StringBuilder [ ] stringBuilders =
				new StringBuilder[ Enum . GetValues ( typeof ( ConfigurationCategory ) ) .
										OfType <ConfigurationCategory> ( ) .
										Max ( type => ( int ) type ) + 1 ] ;

			foreach ( ConfigurationCategory type in Enum . GetValues ( typeof ( ConfigurationCategory ) ) )
			{
				stringBuilders [ ( int ) type ] = new StringBuilder ( ) ;
			}

			foreach ( PropertyInfo property in typeof ( Configurations ) . GetProperties ( ) )
			{
				ConfigurationItemAttribute attribute =
					( ConfigurationItemAttribute ) property . GetCustomAttribute ( typeof ( ConfigurationItemAttribute ) ) ;
				int index = ( int ) attribute . ConfigurationCategory ;
				StringBuilder propertyBuilder = stringBuilders [ index ] ;
				propertyBuilder . AppendLine ( attribute . ToString ( ) ) ;
				propertyBuilder . AppendLine ( $"{property . Name} = {property . GetValue ( this )}" ) ;
				propertyBuilder . AppendLine ( ) ;
			}

			StringBuilder builder = new StringBuilder ( ) ;

			for ( int i = 0 ; i < stringBuilders . Length ; i++ )
			{
				builder . AppendLine ( $"##{( ConfigurationCategory ) i}" ) ;
				builder . AppendLine ( ) ;
				builder . AppendLine ( stringBuilders [ i ] . ToString ( ) ) ;
				builder . AppendLine ( ) ;
			}

			return builder . ToString ( ) ;
		}

		public static Configurations GenerateNew ( )
		{
			Configurations configuration = new Configurations ( ) ;

			foreach ( PropertyInfo property in typeof ( Configurations ) . GetProperties ( ) )
			{
				ConfigurationItemAttribute attribute =
					( ConfigurationItemAttribute ) property . GetCustomAttribute ( typeof ( ConfigurationItemAttribute ) ) ;
				property . SetValue ( configuration , attribute . DefultValue ) ;
			}

			return configuration ;
		}

		public static Configurations Load ( [NotNull] string source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException ( nameof(source) ) ;
			}

			Configurations configurations = new Configurations ( ) ;

			foreach ( string line in source . Split ( new [ ] { System . Environment . NewLine } ,
													StringSplitOptions . RemoveEmptyEntries ) )
			{
				if ( ! string . IsNullOrWhiteSpace ( line ) &&
					! line . StartsWith ( "#" ) )
				{
					string [ ] setCommand = line . Split ( '=' ) ;

					PropertyInfo property = configurations . GetType ( ) .
															GetProperty ( setCommand [ 0 ] . Trim ( ) , BindingFlags . IgnoreCase ) ;
					object value = Convert . ChangeType ( setCommand [ 1 ] . Trim ( ) , property . PropertyType ) ;

					property . SetValue ( configurations , value ) ;
				}
			}

			return configurations ;
		}

		public static Configurations Load ( [NotNull] Stream stream )
		{
			if ( stream == null )
			{
				throw new ArgumentNullException ( nameof(stream) ) ;
			}

			Configurations configurations = new Configurations ( ) ;

			StreamReader reader = new StreamReader ( stream ) ;

			while ( ! reader . EndOfStream )
			{
				string line = reader . ReadLine ( ) ;

				if ( ! string . IsNullOrWhiteSpace ( line ) &&
					! line . StartsWith ( "#" ) )
				{
					string [ ] setCommand = line . Split ( '=' ) ;
					PropertyInfo property = configurations . GetType ( ) .
															GetProperty ( setCommand [ 0 ] . Trim ( ) ) ;

					TypeConverter typeConverter = TypeDescriptor . GetConverter ( property . PropertyType ) ;

					object value = typeConverter . ConvertFromString ( setCommand [ 1 ] . Trim ( ) ) ;

					property . SetValue ( configurations , value ) ;
				}
			}

			reader . Dispose ( ) ;

			return configurations ;
		}

		public class ConfigurationItemAttribute : Attribute
		{

			public ConfigurationCategory ConfigurationCategory { get ; set ; }

			public string DisplayName { get ; set ; }

			public string Introduction { get ; set ; }

			public object DefultValue { get ; set ; }

			public ConfigurationItemAttribute ( ConfigurationCategory configurationCategory ,
												string displayName ,
												string introduction ,
												object defultValue )
			{
				if ( ! Enum . IsDefined ( typeof ( ConfigurationCategory ) , configurationCategory ) )
				{
					throw new ArgumentOutOfRangeException ( nameof(configurationCategory) ,
															"Value should be defined in the ConfigurationCategory enum." ) ;
				}


				ConfigurationCategory = configurationCategory ;
				DisplayName = displayName ?? throw new ArgumentNullException ( nameof(displayName) ) ;
				Introduction = introduction ?? throw new ArgumentNullException ( nameof(introduction) ) ;
				DefultValue = defultValue ?? throw new ArgumentNullException ( nameof(defultValue) ) ;
			}

			public override string ToString ( )
			{
				StringBuilder builder = new StringBuilder ( ) ;
				builder . AppendLine ( $"#	{DisplayName}" ) ;
				builder . AppendLine ( $"#	{Introduction}" ) ;
				builder . AppendLine ( $"#	Defult Value: {DefultValue}" ) ;
				return builder . ToString ( ) ;
			}

		}

	}

}
