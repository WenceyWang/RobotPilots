using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed
{

	public interface ICamera
	{

		bool IsOpened { get ; }

		Size FrameSize { get ; }

		Mat Read ( ) ;

	}

}
