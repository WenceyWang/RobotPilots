using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual
{

	public interface ICalibratedCamera : ICamera
	{

		bool IsCalibrated { get ; }

		ICamera UnderlyingCamera { get ; }

		Mat ReadOriginal ( ) ;

	}

}
