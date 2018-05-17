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

		public abstract Mat Process ( Mat source ) ;

		[Startup]
		public static void LoadPreprocessor ( )
		{
			LoadAll ( ) ;
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
