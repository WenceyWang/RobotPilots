using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Visual
{

	public class ChessBoard
	{

		public Size Size { get ; set ; }

		public float SqureSize { get ; set ; }

		public ChessBoard ( Size size , float squreSize )
		{
			Size = size ;
			SqureSize = squreSize ;
		}

		public List <Point3f> GetObjectPoints ( )
		{
			List <Point3f> result = new List <Point3f> ( Size . Width * Size . Height ) ;

			for ( int y = 0 ; y < Size . Height ; y++ )
			{
				for ( int x = 0 ; x < Size . Width ; x++ )
				{
					result . Add ( new Point3f ( x * Size . Width * SqureSize , y * Size . Height * SqureSize , 0 ) ) ;
				}
			}

			return result ;
		}

		private int ToRawInt32 ( float value )
		{
			byte [ ] raw = BitConverter . GetBytes ( value ) ;
			return BitConverter . ToInt32 ( raw , 0 ) ;
		}

		public Mat GetObjectMat ( )
		{
			List <Point3f> points = GetObjectPoints ( ) ;
			List <int> values = new List <int> ( points . Count * 3 ) ;
			foreach ( Point3f objectPoint in points )
			{
				values . Add ( ToRawInt32 ( objectPoint . X ) ) ;
				values . Add ( ToRawInt32 ( objectPoint . Y ) ) ;
				values . Add ( ToRawInt32 ( objectPoint . Z ) ) ;
			}

			Mat result = new Mat ( values , MatType . CV_32FC3 ) ;

			return result ;
		}

		public Mat FindFromImage ( Mat image )
		{
			Mat imageGray = image . CvtColor ( ColorConversionCodes . BGR2GRAY ) ;

			Mat chessBoardCorners = new Mat ( ) ;
			Cv2 . FindChessboardCorners ( imageGray , Size , chessBoardCorners ) ;

			return chessBoardCorners ;
		}

	}

}
