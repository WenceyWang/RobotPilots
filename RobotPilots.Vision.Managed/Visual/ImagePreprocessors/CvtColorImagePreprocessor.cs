using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual . ImagePreprocessors
{

	[ImagePreprocessor]
	public class CvtColorImagePreprocessor : ImagePreprocessor
	{

		public ColorConversionCodes ColorConversionCodes { get ; }

		public CvtColorImagePreprocessor ( ColorConversionCodes colorConversionCodes )
		{
			ColorConversionCodes = colorConversionCodes ;
		}

		public override Mat Process ( Mat source ) { return source . CvtColor ( ColorConversionCodes ) ; }

	}

}
