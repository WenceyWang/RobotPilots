using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual . ImagePreprocessors
{

	[PublicAPI]
	[ImagePreprocessor]
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
