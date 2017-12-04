using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public class CradleHeadPositionDatagram : ReceiveDatagram
	{

		public AnglePosition Position { get ; set ; }

		public CradleHeadPositionDatagram ( XElement source ) : base ( source ) { }


		public override byte [ ] ToBinary ( ) { throw new NotImplementedException ( ) ; }

	}

}
