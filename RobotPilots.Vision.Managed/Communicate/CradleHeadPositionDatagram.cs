using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Math ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[PublicAPI]
	[Datagram ( nameof(BinaryDatagramType . CradleHeadPosition) , BinaryDatagramType . CradleHeadPosition )]
	public class CradleHeadPositionDatagram : ReceiveDatagram
	{

		public AnglePosition Position { get ; set ; }

		public CradleHeadPositionDatagram ( XElement xmlSource ) : base ( xmlSource )
		{
			Position = new AnglePosition ( ReadNecessaryValue <float> ( xmlSource , nameof(Position . XYaw) ) ,
											ReadNecessaryValue <float> ( xmlSource , nameof(Position . XYaw) ) ) ;
		}

		public CradleHeadPositionDatagram ( byte [ ] binarySource ) : base ( binarySource )
		{
			Position = new AnglePosition ( BitConverter . ToSingle ( binarySource , 0 ) ,
											BitConverter . ToSingle ( binarySource , 4 ) ) ;
		}

		public override byte [ ] ToBinary ( ) { throw new NotImplementedException ( ) ; }

	}

}
