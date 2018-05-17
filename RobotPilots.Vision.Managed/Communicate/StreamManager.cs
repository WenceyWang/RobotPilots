using System ;
using System . Collections ;
using System . Collections . Concurrent ;
using System . Collections . Generic ;
using System . IO ;
using System . Linq ;
using System . Threading ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using Microsoft . Extensions . Logging ;

using RobotPilots . Vision . Managed . Math ;
using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[PublicAPI]
	public class StreamManager
	{

		private static ILogger Logger { get ; set ; }

		public ConcurrentQueue <SendDatagram> SendQueue { get ; } = new ConcurrentQueue <SendDatagram> ( ) ;

		public ConcurrentQueue <ReceiveDatagram> ReceiveQueue { get ; } = new ConcurrentQueue <ReceiveDatagram> ( ) ;

		public Stream UnderlyingStream { get ; }

		public bool IsRunning { get ; private set ; }

		public Thread ListenThread { get ; private set ; }

		public Thread SendThread { get ; private set ; }

		public Thread ProcessThread { get ; private set ; }

		public SerializationMode SendMode { get ; }

		public SerializationMode ReceiveMode { get ; }

		public StreamManager ( [NotNull] Stream underlyingStream ,
									SerializationMode receiveMode = SerializationMode . Binary ,
									SerializationMode sendMode = SerializationMode . Binary )
		{
			UnderlyingStream = underlyingStream ?? throw new ArgumentNullException ( nameof(underlyingStream) ) ;
			ReceiveMode = receiveMode ;
			SendMode = sendMode ;
		}

		public const byte DatagramHeader = 0xAA ;

		public const int DatagramHeaderInt = 0xAA ;

		[Startup]
		public static void Startup ( )
		{
			Logger = Application . LoggerFactory . CreateLogger ( typeof ( StreamManager ) ) ;
		}

		public void SendDatagram ( [NotNull] SendDatagram datagram )
		{
			if ( datagram == null )
			{
				throw new ArgumentNullException ( nameof(datagram) ) ;
			}

			SendQueue . Enqueue ( datagram ) ;
		}

		public void Stop ( )
		{
			lock ( this )
			{
				if ( IsRunning )
				{
					ListenThread . Abort ( ) ;
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
							ListenThread = new Thread ( BinaryListenTask ) ;
							break ;
						}
						default :
						{
							throw new ArgumentOutOfRangeException ( ) ;
						}
					}

					Logger . LogInformation ( $"{ReceiveMode} Receive Opened" ) ;

					switch ( SendMode )
					{
						case SerializationMode . Xml :
						{
							SendThread = new Thread ( XmlSendTask ) ;
							break ;
						}
						case SerializationMode . Binary :
						{
							SendThread = new Thread ( BinarySendTask ) ;
							break ;
						}
						default :
						{
							throw new ArgumentOutOfRangeException ( ) ;
						}
					}

					Logger . LogInformation ( $"{SendMode} Send Opened" ) ;


					ProcessThread = new Thread ( ProcessTask ) ;

					Logger . LogInformation ( "Process Opened" ) ;


					ListenThread . Start ( ) ;
					SendThread . Start ( ) ;
					ProcessThread . Start ( ) ;

					IsRunning = true ;
				}
			}
		}

		public void ProcessTask ( )
		{
			while ( IsRunning )
			{
				if ( ReceiveQueue . TryDequeue ( out ReceiveDatagram datagram ) )
				{
					DatagramReceived ? . Invoke ( this , new ReceiveDatagramEventArgs ( datagram ) ) ;
				}
				else
				{
					Thread . Sleep ( 20 ) ;
				}
			}
		}

		public event EventHandler <ReceiveDatagramEventArgs> DatagramReceived ;

		public void XmlListenTask ( )
		{
			try
			{
				StreamReader reader = new StreamReader ( UnderlyingStream ) ;

				while ( IsRunning )
				{
					string datagramString = reader . ReadLine ( ) ;

					XElement element = XElement . Parse ( datagramString ) ;

					if ( Datagram . Parse ( element ) is ReceiveDatagram datagram )
					{
						ReceiveQueue . Enqueue ( datagram ) ;
					}
				}
			}
			catch ( ThreadAbortException )
			{
			}
		}

		public void BinaryListenTask ( )
		{
			try
			{
				byte currentSequence = 0 ;
				while ( IsRunning )
				{
					if ( UnderlyingStream . ReadByte ( ) == DatagramHeaderInt )
					{
						byte sequence = ( byte ) UnderlyingStream . ReadByte ( ) ;

						if ( sequence == currentSequence )
						{
							currentSequence++ ;
						}
						else
						{
							//Todo:???
						}

						BinaryDatagramType type = ( BinaryDatagramType ) ( byte ) UnderlyingStream . ReadByte ( ) ; //todo:if not throw

						byte crc = ( byte ) UnderlyingStream . ReadByte ( ) ;

						byte length = ( byte ) UnderlyingStream . ReadByte ( ) ;

						byte [ ] data = new byte[ length ] ;

						UnderlyingStream . Read ( data , 0 , length ) ;

						if ( data . CaluCrc8 ( ) == crc )
						{
							if ( Datagram . Parse ( type , data ) is ReceiveDatagram datagram )
							{
								ReceiveQueue . Enqueue ( datagram ) ;
							}
						}
						else
						{
							//todo:Warning?
						}
					}
				}
			}
			catch ( ThreadAbortException )
			{
			}
		}

		public void BinarySendTask ( )
		{
			byte sequence = 0 ;
			while ( IsRunning )
			{
				if ( SendQueue . TryDequeue ( out SendDatagram datagram ) )
				{
					byte [ ] data = datagram . ToBinary ( ) ;

					UnderlyingStream . WriteByte ( DatagramHeader ) ;
					UnderlyingStream . WriteByte ( sequence ) ;
					UnderlyingStream . WriteByte ( ( byte ) datagram . Type . BinaryType ) ;
					UnderlyingStream . WriteByte ( data . CaluCrc8 ( ) ) ;
					UnderlyingStream . WriteByte ( Convert . ToByte ( data . Length ) ) ;
					UnderlyingStream . Write ( data , 0 , data . Length ) ;

					sequence++ ;
				}
				else
				{
					Thread . Sleep ( 20 ) ;
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
					writer . WriteLine ( datagram ) ;
				}
				else
				{
					Thread . Sleep ( 20 ) ;
				}
			}
		}

	}

}
