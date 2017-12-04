using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual
{

	public class CalibratedCamera : ICalibratedCamera
	{

		public Mat CameraMatrix { get ; set ; }

		public Mat DistortionCoefficients { get ; set ; }

		public CalibratedCamera ( ICamera underlyingCamera ) { UnderlyingCamera = underlyingCamera ; }

		public bool IsOpened => UnderlyingCamera != null ;

		public Size FrameSize => UnderlyingCamera . FrameSize ;

		public Mat Read ( )
		{
			Mat image = UnderlyingCamera ? . Read ( ) ;
			if ( IsCalibrated )
			{
				return image ? . Undistort ( CameraMatrix , DistortionCoefficients ) ;
			}
			else
			{
				return image ;
			}
		}

		public bool IsCalibrated => CameraMatrix != null && DistortionCoefficients != null ;

		public Mat ReadOriginal ( ) { return UnderlyingCamera ? . Read ( ) ; }

		public ICamera UnderlyingCamera { get ; set ; }

		public void CalibrateFromImages ( List <Mat> images , ChessBoard chessBoard )
		{
			List <Mat> objectPointsOfFrames = new List <Mat> ( images . Count ) ;
			List <Mat> imagePointsOfFrames = new List <Mat> ( images . Count ) ;

			foreach ( Mat image in images )
			{
				imagePointsOfFrames . Add ( chessBoard . FindFromImage ( image ) ) ;
				objectPointsOfFrames . Add ( chessBoard . GetObjectMat ( ) ) ;
			}

			Cv2 . CalibrateCamera ( objectPointsOfFrames ,
									imagePointsOfFrames ,
									FrameSize ,
									CameraMatrix ,
									DistortionCoefficients ,
									out Mat [ ] rotationVectors ,
									out Mat [ ] translationVector ) ;
		}

		public void CalibrateFromPoints ( IEnumerable <Mat> objectPointsOfFrames , IEnumerable <Mat> imagePointsOfFrames )
		{
			Cv2 . CalibrateCamera ( objectPointsOfFrames ,
									imagePointsOfFrames ,
									FrameSize ,
									CameraMatrix ,
									DistortionCoefficients ,
									out Mat [ ] rotationVectors ,
									out Mat [ ] translationVector ) ;
		}

	}

}
