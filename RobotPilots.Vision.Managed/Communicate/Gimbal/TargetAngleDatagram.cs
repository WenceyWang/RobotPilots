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
	[Datagram ( nameof(BinaryDatagramType . TargetAngle) , BinaryDatagramType . TargetAngle )]
	public class TargetAngleDatagram : SendDatagram
	{

		public AnglePosition Position { get ; }

		public TargetAngleDatagram ( AnglePosition position ) { Position = position ; }

		public override XElement ToXElement ( )
		{
			XElement result = base . ToXElement ( ) ;

			result . SetAttributeValue ( nameof(Position . XYaw) , Position . XYaw . FloatDegree ) ;
			result . SetAttributeValue ( nameof(Position . YPitch) , Position . YPitch . FloatDegree ) ;

			return result ;
		}

		public override byte [ ] ToBinary ( )
		{
			List <byte> byties = new List <byte> ( 8 ) ;

			byties . AddRange ( BitConverter . GetBytes ( Position . XYaw . FloatDegree ) ) ;
			byties . AddRange ( BitConverter . GetBytes ( Position . YPitch . FloatDegree ) ) ;

			return byties . ToArray ( ) ;
		}

	}

}
