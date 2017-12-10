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

	[PublicAPI]
	public class TrafficManager
	{

		public static TrafficManager Current { get ; }


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

		public void Stop ( )
		{
			lock ( this )
			{
				if ( IsRunning )
				{
					IsRunning = false ;
				}
			}
		}

		public void Run ( )
		{
			lock ( this )
			{
				if ( ! IsRunning )
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
		}

		public event EventHandler <ReceivePackageEventArgs> PackageReceived ;


		public void XmlListenTask ( )
		{
			StreamReader reader = new StreamReader ( UnderlyingStream ) ;

			while ( IsRunning )
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
			while ( IsRunning )
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
			while ( IsRunning )
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

			while ( IsRunning )
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

}
