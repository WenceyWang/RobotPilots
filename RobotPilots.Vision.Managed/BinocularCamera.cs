using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed
{

	public class BinocularCamera : IBinocularCamera
	{

		public CalibratedCamera LeftCamera { get ; set ; }

		public CalibratedCamera RightCamera { get ; set ; }

		/// <summary>
		/// </summary>
		public StereoMatcher Stereo { get ; set ; }

		/// <summary>
		///     Rotation matrix between the 1st and the 2nd camera coordinate systems.
		/// </summary>
		public Mat RotationVector { get ; set ; }

		/// <summary>
		///     Rectification transform (rotation matrix) for the left camera.
		/// </summary>
		public Mat LeftRotationVector { get ; set ; }

		/// <summary>
		///     Rectification transform (rotation matrix) for the right camera.
		/// </summary>
		public Mat LeftTranslationVector { get ; set ; }

		public Mat RightRotationVector { get ; set ; }

		public Mat RightTranslationVector { get ; set ; }

		public Mat DisparityToDepthMappingMatrix { get ; set ; }

		/// <summary>
		///     Translation vector between the coordinate systems of the cameras.
		/// </summary>
		public Mat TranslationVector { get ; set ; }

		public Mat LeftMapX { get ; set ; }

		public Mat LeftMapY { get ; set ; }

		public Mat RightMapX { get ; set ; }

		public Mat RightMapY { get ; set ; }

		public BinocularCamera ( ICamera leftCamera , ICamera rightCamera )
		{
			LeftCamera = new CalibratedCamera ( leftCamera ) ;
			RightCamera = new CalibratedCamera ( rightCamera ) ;
		}

		public bool IsOpened => LeftCamera . IsOpened && RightCamera . IsOpened ;

		public Size FrameSize => LeftCamera . FrameSize ;

		public Mat Read ( )
		{
			Mat leftFrame = LeftCamera . ReadOriginal ( ) . Remap ( LeftMapX , LeftMapY ) ;
			Mat rightFrame = RightCamera . ReadOriginal ( ) . Remap ( RightMapX , RightMapY ) ;

			Mat disparity = new Mat ( ) ;
			Stereo . Compute ( leftFrame , rightFrame , disparity ) ;

			Mat depth = new Mat ( ) ;
			Cv2 . ReprojectImageTo3D ( disparity , depth , DisparityToDepthMappingMatrix ) ;

			return depth ;
		}

		public void StereoCalibrateFromImages ( List <(Mat , Mat)> images , ChessBoard chessBoard )
		{
			//StereoSGBM

			List <InputArray> objectPointsOfFrames = new List <InputArray> ( images . Count ) ;

			List <InputArray> leftImagePointsOfFrames = new List <InputArray> ( images . Count ) ;
			List <InputArray> rightImagePointsOfFrames = new List <InputArray> ( images . Count ) ;

			foreach ( (Mat left , Mat right) image in images )
			{
				leftImagePointsOfFrames . Add ( chessBoard . FindFromImage ( image . left ) ) ;
				rightImagePointsOfFrames . Add ( chessBoard . FindFromImage ( image . right ) ) ;
				objectPointsOfFrames . Add ( chessBoard . GetObjectMat ( ) ) ;
			}

			Mat essentialMatrix = new Mat ( ) , fundamentalMatrix = new Mat ( ) ;

			Cv2 . StereoCalibrate (
				objectPointsOfFrames ,
				leftImagePointsOfFrames ,
				rightImagePointsOfFrames ,
				LeftCamera . CameraMatrix ,
				LeftCamera . DistortionCoefficients ,
				RightCamera . CameraMatrix ,
				RightCamera . DistortionCoefficients ,
				FrameSize ,
				RotationVector ,
				TranslationVector ,
				essentialMatrix ,
				fundamentalMatrix ,
				CalibrationFlags . None ) ;

			Cv2 . StereoRectify (
				LeftCamera . CameraMatrix ,
				LeftCamera . DistortionCoefficients ,
				RightCamera . CameraMatrix ,
				RightCamera . DistortionCoefficients ,
				FrameSize ,
				RotationVector ,
				TranslationVector ,
				LeftRotationVector ,
				RightRotationVector ,
				LeftTranslationVector ,
				RightTranslationVector ,
				DisparityToDepthMappingMatrix ) ;

			Cv2 . InitUndistortRectifyMap (
				LeftCamera . CameraMatrix ,
				LeftCamera . DistortionCoefficients ,
				LeftRotationVector ,
				LeftTranslationVector ,
				FrameSize ,
				MatType . CV_16SC2 ,
				LeftMapX ,
				LeftMapY ) ;

			Cv2 . InitUndistortRectifyMap (
				RightCamera . CameraMatrix ,
				RightCamera . DistortionCoefficients ,
				RightRotationVector ,
				RightTranslationVector ,
				FrameSize ,
				MatType . CV_16SC2 ,
				RightMapX ,
				RightMapY ) ;
		}

	}

}
