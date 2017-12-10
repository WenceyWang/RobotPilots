using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public static class Crc8Helper
	{

		static Crc8Helper ( )
		{
			for ( int i = 0 ; i < 256 ; ++i )
			{
				int temp = i ;
				for ( int j = 0 ; j < 8 ; ++j )
				{
					if ( ( temp & 0x80 ) != 0 )
					{
						temp = ( temp << 1 ) ^ Poly ;
					}
					else
					{
						temp <<= 1 ;
					}
				}

				Table [ i ] = ( byte ) temp ;
			}
		}

		// x8 + x7 + x6 + x4 + x2 + 1
		private const byte Poly = 0xd5 ;

		private static readonly byte [ ] Table = new byte[ 256 ] ;

		public static byte CaluCrc8 ( this byte [ ] data ) { return ComputeChecksum ( data ) ; }

		public static byte ComputeChecksum ( params byte [ ] data )
		{
			byte crc = 0 ;
			if ( data != null &&
				data . Length > 0 )
			{
				foreach ( byte b in data )
				{
					crc = Table [ crc ^ b ] ;
				}
			}

			return crc ;
		}

	}

}
