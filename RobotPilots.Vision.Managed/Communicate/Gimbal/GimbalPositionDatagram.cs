using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Math ;

namespace RobotPilots . Vision . Managed . Communicate . Gimbal
{

	[PublicAPI]
	[Datagram ( nameof(BinaryDatagramType . GimbalPosition) , BinaryDatagramType . GimbalPosition )]
	public class GimbalPositionDatagram : ReceiveDatagram
	{

		public AnglePosition Position { get ; set ; }

		public GimbalPositionDatagram ( XElement xmlSource ) : base ( xmlSource )
		{
			Position = new AnglePosition ( ReadNecessaryValue <float> ( xmlSource , nameof(Position . XYaw) ) ,
											ReadNecessaryValue <float> ( xmlSource , nameof(Position . XYaw) ) ) ;
		}

		public GimbalPositionDatagram ( byte [ ] binarySource ) : base ( binarySource )
		{
			Position = new AnglePosition ( BitConverter . ToSingle ( binarySource , 0 ) ,
											BitConverter . ToSingle ( binarySource , 4 ) ) ;
		}

	}

}
