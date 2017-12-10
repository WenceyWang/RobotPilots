using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[PublicAPI]
	[Datagram ( nameof(BinaryDatagramType . TargetPosition) , BinaryDatagramType . TargetPosition )]
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

		public override byte [ ] ToBinary ( )
		{
			List <byte> byties = new List <byte> ( 12 ) ;

			byties . AddRange ( BitConverter . GetBytes ( Point . X ) ) ;
			byties . AddRange ( BitConverter . GetBytes ( Point . Y ) ) ;
			byties . AddRange ( BitConverter . GetBytes ( Point . Z ) ) ;

			return byties . ToArray ( ) ;
		}

	}

}
