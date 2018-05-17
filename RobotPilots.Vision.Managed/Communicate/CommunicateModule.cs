using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

using JetBrains.Annotations;

using RJCP.IO.Ports;

using RobotPilots.Vision.Managed.Utility;

namespace RobotPilots . Vision . Managed . Communicate
{

	public class CommunicateServer
	{

		public TcpListener Listener { get ; private set ; }

		public CommunicateServer ( IPAddress localAddress , int port )
		{
			Listener = new TcpListener ( localAddress , port ) ;
		}

		public List <StreamManager> Clients { get ; } = new List <StreamManager> ( ) ;

		public void Run ( ) { }

		public void BroadcastPackage ( SendDatagram datagram )
		{
			foreach ( StreamManager manager in Clients )
			{
				manager . SendDatagram ( datagram ) ;
			}
		}


		public X509Certificate ServerCertificate { get ; set ; } = new X509Certificate ( ) ;

		public void ProcessIncomingConnection ( )
		{
			while ( true )
			{
				try
				{
					TcpClient client = Listener . AcceptTcpClient ( ) ;
					client . GetStream ( ) ;

					SslStream stream = new SslStream (
						client . GetStream ( ) ,
						false ) ;

					stream . AuthenticateAsServer ( ServerCertificate ,
													false ,
													SslProtocols . Tls ,
													true ) ;

					Clients . Add ( new StreamManager ( stream ) ) ;
				}
				catch ( ThreadAbortException )
				{
				}
			}
		}


	}

	[Module]
	[PublicAPI]
	public class CommunicateModule : IModule
	{

		public static CommunicateModule Current { get ; set ; }

		public StreamManager SerialManager { get ; set ; }

		public StreamManager SocketManager { get ; set ; }

		public CommunicateServer SocketServer { get ; set ; }

		public string [ ] Dependencies { get ; } = { } ;

		public void Prepare ( Configurations configuration )
		{
			Current = this ;

			SerialPortStream serialPortStream = new SerialPortStream ( configuration . SerialPortName ,
																		configuration . SerialPortBaudRate ,
																		configuration . SerialPortDataBits ,
																		configuration . SerialPortParity ,
																		configuration . SerialPortStopBits ) ;

			serialPortStream . Open ( ) ;

			SerialManager = new StreamManager ( serialPortStream , configuration . ReceiveMode , configuration . SendMode ) ;

			SerialManager . Run ( ) ;

			if ( configuration . IsStatusServer )
			{
				TcpListener listener = new TcpListener ( IPAddress . Any , configuration . StatusPort ) ;
			}

			else
			{
				TcpClient client = new TcpClient ( configuration . StatusServerHostName , configuration . StatusPort ) ;

				SslStream sslStream = new SslStream ( client . GetStream ( ) ,
													false ,
													ValidateServerCertificate ,
													null ) ;

				sslStream . AuthenticateAsClient ( configuration . StatusServerHostName ) ;

				SocketManager = new StreamManager ( sslStream , configuration . ReceiveMode , configuration . SendMode ) ;
			}
		}

		public static bool ValidateServerCertificate ( object sender ,
														X509Certificate certificate ,
														X509Chain chain ,
														SslPolicyErrors sslPolicyErrors )
		{
			return true ;
		}

		public void Dispose ( )
		{
			SerialManager ? . Stop ( ) ;
			SerialManager ? . UnderlyingStream ? . Close ( ) ;
		}

	}

}
