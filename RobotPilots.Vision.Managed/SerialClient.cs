using System ;
using System . Collections ;
using System . Collections . Concurrent ;
using System . Collections . Generic ;
using System . IO ;
using System . Linq ;
using System . Threading ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed
{

	public interface ISelfSerializeable
	{

		/// <summary>
		/// </summary>
		/// <returns></returns>
		[NotNull]
		XElement ToXElement ( ) ;

	}

	public abstract class Datagram : NeedRegisBase <DatagramType , DatagramAttribute , Datagram> , ISelfSerializeable
	{

		public abstract DateTimeOffset ? Time { get ; set ; }

		public abstract XElement ToXElement ( ) ;

		public static Datagram Parse ( XElement element )
		{
			return Create ( TypeList . Single ( type => type . XmlName == element . Name ) , element ) ;
		}

		public abstract byte [ ] ToBinary ( ) ;

		public override string ToString ( ) { return ToXElement ( ) . ToString ( ) ; }

		[Startup]
		public static void LoadDatagram ( )
		{
		}

	}


	public class DatagramAttribute : NeedRegisAttributeBase
	{

		public string XmlName { get ; }

		public DatagramAttribute ( string xmlName ) { XmlName = xmlName ; }

	}

	public class DatagramType : RegisType <DatagramType , DatagramAttribute , Datagram>
	{

		public string XmlName { get ; }

		public DatagramType ( [NotNull] Type entryType ) : base ( entryType )
		{
			XmlName = ( ( DatagramAttribute ) entryType . GetCustomAttributes ( typeof ( DatagramAttribute ) , false ) .
														First ( ) ) . XmlName ;
		}

	}

	[Datagram ( "TargetPosition" )]
	public class TargetPositionDatagram : SendDatagram
	{

		public Point3f Point { get ; set ; }

		public TargetPositionDatagram ( Point3f point ) { Point = point ; }

		public override XElement ToXElement ( )
		{
			XElement result = base . ToXElement ( ) ;

			result . SetAttributeValue ( nameof(Point . X) , Point . X ) ;
			result . SetAttributeValue ( nameof(Point . Y) , Point . Y ) ;
			result . SetAttributeValue ( nameof(Point . Z) , Point . Z ) ;

			return result ;
		}

		public override byte [ ] ToBinary ( ) { throw new NotImplementedException ( ) ; }

	}


	public class CradleHeadPositionDatagram : ReceiveDatagram
	{

		public AnglePosition Position { get ; set ; }

		public CradleHeadPositionDatagram ( XElement source ) : base ( source ) { }


		public override byte [ ] ToBinary ( ) { throw new NotImplementedException ( ) ; }

	}

	public abstract class SendDatagram : Datagram
	{

		public override DateTimeOffset ? Time { get ; set ; }

		public override XElement ToXElement ( )
		{
			XElement result = new XElement ( Type . XmlName ) ;

			result . SetAttributeValue ( nameof(Time) , Time ? . ToUnixTimeMilliseconds ( ) ) ;

			return result ;
		}

		public virtual void PrepareForSend ( ) { Time = DateTimeOffset . Now ; }

	}

	public abstract class ReceiveDatagram : Datagram
	{

		public XElement Source { get ; }

		public sealed override DateTimeOffset ? Time { get ; set ; }

		protected ReceiveDatagram ( XElement source )
		{
			Source = source ;
			Time = DateTimeOffset . FromUnixTimeMilliseconds (
				Convert . ToInt64 ( source . Attribute ( nameof(Time) ) ? . Value ) ) ;
		}

		public sealed override XElement ToXElement ( ) { return Source ; }

	}

	public class ReceivePackageEventArgs : EventArgs
	{

		public ReceiveDatagram Datagram { get ; set ; }

		public ReceivePackageEventArgs ( ReceiveDatagram datagram ) { Datagram = datagram ; }

	}

	public class TrafficManager
	{

		public ConcurrentQueue <SendDatagram> SendQueue { get ; } = new ConcurrentQueue <SendDatagram> ( ) ;

		public ConcurrentQueue <ReceiveDatagram> ReceiveQueue { get ; } = new ConcurrentQueue <ReceiveDatagram> ( ) ;

		public Stream UnderlyingStream { get ; }

		public bool IsRunning { get ; private set ; }

		public Thread ListenThread { get ; private set ; }

		public Thread SendThread { get ; private set ; }

		public TrafficManager ( Stream underlyingStream ) { UnderlyingStream = underlyingStream ; }

		public void Run ( )
		{
			lock ( this )
			{
				ListenThread = new Thread ( ListenTask ) ;
				SendThread = new Thread ( ListenTask ) ;
				ListenThread . Start ( ) ;
				SendThread . Start ( ) ;

				IsRunning = true ;
			}
		}

		public event EventHandler <ReceivePackageEventArgs> PackageReceived ;

		public void ListenTask ( )
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

		public void SendTask ( )
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

}
