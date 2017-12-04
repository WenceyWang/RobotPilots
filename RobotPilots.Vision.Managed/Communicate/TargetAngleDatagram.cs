﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	[Datagram ( nameof(BinaryDatagramType . TargetAngle) , BinaryDatagramType . TargetAngle )]
	public class TargetAngleDatagram : SendDatagram
	{

		public AnglePosition Position { get ; }

		public override XElement ToXElement ( )
		{
			XElement result = base . ToXElement ( ) ;

			result . SetAttributeValue ( nameof(Position . XPitch) , Position . XPitch . FloatDegree ) ;
			result . SetAttributeValue ( nameof(Position . YPitch) , Position . YPitch . FloatDegree ) ;

			return result ;
		}

		public override byte [ ] ToBinary ( )
		{
			List <byte> byties = new List <byte> ( 8 ) ;

			byties . AddRange ( BitConverter . GetBytes ( Position . XPitch . FloatDegree ) ) ;
			byties . AddRange ( BitConverter . GetBytes ( Position . YPitch . FloatDegree ) ) ;

			return byties . ToArray ( ) ;
		}

	}

}
