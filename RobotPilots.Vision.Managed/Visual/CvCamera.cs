using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual
{

	public class CvCamera : ICamera
	{

		public VideoCapture VideoCapture { get ; set ; }

		public CvCamera ( int index ) : this ( VideoCapture . FromCamera ( index ) ) { }

		public CvCamera(string fileName) : this(VideoCapture.FromFile(fileName)) { }


		public CvCamera ( VideoCapture videoCapture )
		{
			VideoCapture = videoCapture ?? throw new ArgumentNullException ( nameof(videoCapture) ) ;
		}

		public bool IsOpened => VideoCapture != null ;

		public Size FrameSize => new Size ( VideoCapture . FrameWidth , VideoCapture . FrameHeight ) ;

		public Mat Read ( )
		{
			Mat result = new Mat ( ) ;
			if ( VideoCapture ? . Read ( result ) ?? false )
			{
				return result ;
			}
			else
			{
				return null ;
			}
		}

	}

}
