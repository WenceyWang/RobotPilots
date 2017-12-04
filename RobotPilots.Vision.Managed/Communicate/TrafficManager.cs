using System ;
using System . Collections ;
using System . Collections . Concurrent ;
using System . Collections . Generic ;
using System . IO ;
using System . Linq ;
using System . Threading ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public static class Crc8
	{

		static Crc8 ( )
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

	public class TrafficManager
	{

		public ConcurrentQueue <SendDatagram> SendQueue { get ; } = new ConcurrentQueue <SendDatagram> ( ) ;

		public ConcurrentQueue <ReceiveDatagram> ReceiveQueue { get ; } = new ConcurrentQueue <ReceiveDatagram> ( ) ;

		public Stream UnderlyingStream { get ; }

		public bool IsRunning { get ; private set ; }

		public Thread ListenThread { get ; private set ; }

		public Thread SendThread { get ; private set ; }

		public SerializationMode SendMode { get ; }

		public SerializationMode ReceiveMode { get ; }

		public TrafficManager ( [NotNull] Stream underlyingStream ,
								SerializationMode receiveMode = SerializationMode . Binary ,
								SerializationMode sendMode = SerializationMode . Binary )
		{
			UnderlyingStream = underlyingStream ?? throw new ArgumentNullException ( nameof(underlyingStream) ) ;
			ReceiveMode = receiveMode ;
			SendMode = sendMode ;
		}

		public const byte PackageHeader = 0xAA ;

		public const byte PackageHeaderInt = 0xAA ;

		public void Run ( )
		{
			lock ( this )
			{
				switch ( ReceiveMode )
				{
					case SerializationMode . Xml :
					{
						ListenThread = new Thread ( XmlListenTask ) ;
						break ;
					}
					case SerializationMode . Binary :
					{
						break ;
					}
					default :
					{
						throw new ArgumentOutOfRangeException ( ) ;
					}
				}

				switch ( SendMode )
				{
					case SerializationMode . Xml :
					{
						SendThread = new Thread ( XmlSendTask ) ;
						break ;
					}
					case SerializationMode . Binary :
					{
						break ;
					}
					default :
					{
						throw new ArgumentOutOfRangeException ( ) ;
					}
				}

				ListenThread . Start ( ) ;
				SendThread . Start ( ) ;

				IsRunning = true ;
			}
		}

		public event EventHandler <ReceivePackageEventArgs> PackageReceived ;


		public void XmlListenTask ( )
		{
			StreamReader reader = new StreamReader ( UnderlyingStream ) ;

			while ( true )
			{
				string package = reader . ReadLine ( ) ;

				XElement element = XElement . Parse ( package ) ;

				if ( Datagram . Parse ( element ) is ReceiveDatagram datagram )
				{
					ReceiveQueue . Enqueue ( datagram ) ;
				}
			}
		}

		public void BinaryListenTask ( )
		{
			while ( true )
			{
				if ( UnderlyingStream . ReadByte ( ) == PackageHeaderInt )
				{
					byte sequence = ( byte ) UnderlyingStream . ReadByte ( ) ;

					BinaryDatagramType type = ( BinaryDatagramType ) ( byte ) UnderlyingStream . ReadByte ( ) ; //todo:if not throw

					byte crc = ( byte ) UnderlyingStream . ReadByte ( ) ;

					byte length = ( byte ) UnderlyingStream . ReadByte ( ) ;

					byte [ ] data = new byte[ length ] ;

					UnderlyingStream . Read ( data , 0 , length ) ;

					if ( data . CaluCrc8 ( ) == crc )
					{
					}
				}
			}
		}

		public void BinarySendTask ( )
		{
			byte sequence = 0 ;
			while ( true )
			{
				if ( SendQueue . TryDequeue ( out SendDatagram datagram ) )
				{
					Thread . Sleep ( 20 ) ;
				}
				else
				{
					byte [ ] data = datagram . ToBinary ( ) ;

					UnderlyingStream . WriteByte ( PackageHeader ) ;
					UnderlyingStream . WriteByte ( sequence ) ;
					UnderlyingStream . WriteByte ( ( byte ) datagram . Type . BinaryType ) ;
					UnderlyingStream . WriteByte ( data . CaluCrc8 ( ) ) ;
					UnderlyingStream . WriteByte ( Convert . ToByte ( data . Length ) ) ;
					UnderlyingStream . Write ( data , 0 , data . Length ) ;
				}
			}
		}

		public void XmlSendTask ( )
		{
			StreamWriter writer = new StreamWriter ( UnderlyingStream ) ;

			while ( true )
			{
				if ( SendQueue . TryDequeue ( out SendDatagram datagram ) )
				{
					Thread . Sleep ( 20 ) ;
				}
				else
				{
					writer . WriteLine ( datagram ) ;
				}
			}
		}

	}

	public enum SerializationMode
	{

		Xml ,

		Binary

	}

}
