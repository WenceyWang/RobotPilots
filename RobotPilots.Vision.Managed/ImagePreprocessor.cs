using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed
{

	public abstract class ImagePreprocessor : NeedRegisBase <ImagePreprocessor . ImagePreprocessorType ,
		ImagePreprocessor . ImagePreprocessorAttribute , ImagePreprocessor>
	{

		public abstract Mat Process ( Mat source ) ;

		public class ImagePreprocessorAttribute : NeedRegisAttributeBase
		{

		}

		public class
			ImagePreprocessorType : RegisType <ImagePreprocessorType , ImagePreprocessorAttribute , ImagePreprocessor>
		{

			public ImagePreprocessorType ( [NotNull] Type entryType ) : base ( entryType ) { }

		}

	}


	public sealed class CompositeImagePreprocessor : ImagePreprocessor
	{

		public List <ImagePreprocessor> ImagePreprocessors { get ; set ; } = new List <ImagePreprocessor> ( ) ;

		public override Mat Process ( Mat source )
		{
			Mat current = source ;
			foreach ( ImagePreprocessor imagePreprocessor in ImagePreprocessors )
			{
				current = imagePreprocessor . Process ( current ) ;
			}

			return current ;
		}

	}

}
