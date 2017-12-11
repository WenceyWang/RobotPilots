using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Visual . Detectors . Test
{

	[Detector]
	public class ChessBoardDetector : Detector
	{

		public ChessBoard ChessBoard { get ; }

		public ChessBoardDetector ( [NotNull] ChessBoard chessBoard )
		{
			ChessBoard = chessBoard ?? throw new ArgumentNullException ( nameof(chessBoard) ) ;
		}

		public override List <Point2f> Detcet ( Mat frame )
		{
			using ( Mat pointResult = new Mat ( ) )
			{
				Cv2 . FindChessboardCorners ( frame , ChessBoard . Size , pointResult ) ;
				return pointResult . ToList <Point2f> ( ) ;
			}
		}

	}

}
