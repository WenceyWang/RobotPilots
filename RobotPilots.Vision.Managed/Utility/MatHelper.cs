using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Utility
{

	public static class MatHelper
	{

		public static List <T> ToList <T> ( this Mat mat ) where T : struct
		{
			List <T> result = new List <T> ( mat . Height * mat . Width ) ;
			for ( int y = 0 ; y < mat . Height ; y++ )
			{
				for ( int x = 0 ; x < mat . Width ; x++ )
				{
					result . Add ( mat . At <T> ( y , x ) ) ;
				}
			}

			return result ;
		}

	}

}
