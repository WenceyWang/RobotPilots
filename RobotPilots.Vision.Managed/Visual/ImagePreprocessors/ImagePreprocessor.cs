using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Visual . ImagePreprocessors
{

	public abstract class ImagePreprocessor : NeedRegisBase <ImagePreprocessor . ImagePreprocessorType ,
		ImagePreprocessor . ImagePreprocessorAttribute , ImagePreprocessor>
	{

		public static bool Loaded { get ; private set ; }

		public abstract Mat Process ( Mat source ) ;

		[Startup]
		public static void LoadPreprocessor ( )
		{
			lock ( Locker )
			{
				if ( Loaded )
				{
					return ;
				}

				//Todo:Load All internal type
				foreach ( TypeInfo type in typeof ( ImagePreprocessor ) . GetTypeInfo ( ) .
																		Assembly . DefinedTypes .
																		Where ( type => type . GetCustomAttributes ( typeof ( ImagePreprocessorAttribute ) , false ) . Any ( )
																						&& typeof ( ImagePreprocessor ) . GetTypeInfo ( ) . IsAssignableFrom ( type ) ) )
				{
					RegisType ( type . AsType ( ) ) ; //Todo:resources?
				}

				Loaded = true ;
			}
		}

		public class ImagePreprocessorAttribute : NeedRegisAttributeBase
		{

		}

		public class
			ImagePreprocessorType : RegisType <ImagePreprocessorType , ImagePreprocessorAttribute , ImagePreprocessor>
		{

			public ImagePreprocessorType ( [NotNull] Type entryType ) : base ( entryType ) { }

		}

	}

}
