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
	public class CradleHeadTargetDatagram : ReceiveDatagram
	{

		public AnglePosition Target { get ; set ; }

		public CradleHeadTargetDatagram ( XElement xmlSource ) : base ( xmlSource )
		{
			Target = new AnglePosition ( ReadNecessaryValue <float> ( xmlSource , nameof(Target . XYaw) ) ,
										ReadNecessaryValue <float> ( xmlSource , nameof(Target . XYaw) ) ) ;
		}

		public CradleHeadTargetDatagram ( byte [ ] binarySource ) : base ( binarySource )
		{
			Target = new AnglePosition ( BitConverter . ToSingle ( binarySource , 0 ) ,
										BitConverter . ToSingle ( binarySource , 4 ) ) ;
		}

	}

}
