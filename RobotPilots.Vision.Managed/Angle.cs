using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed
{

	public struct Angle : IEquatable <Angle>
	{

		public static implicit operator Angle ( double degree ) { return new Angle ( degree ) ; }

		public static implicit operator double ( Angle angle ) { return angle . Degree ; }

		public double Degree { get ; }

		public bool Equals ( Angle other ) { return Degree . Equals ( other . Degree ) ; }

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
			{
				return false ;
			}

			return obj is Angle angle && Equals ( angle ) ;
		}

		public override int GetHashCode ( ) { return Degree . GetHashCode ( ) ; }

		public static bool operator == ( Angle left , Angle right ) { return left . Equals ( right ) ; }

		public static bool operator != ( Angle left , Angle right ) { return ! left . Equals ( right ) ; }

		public double Radius => Degree / 180 * Math . PI ;

		public Angle ( double degree ) { Degree = degree ; }

		public double Grad => Degree / 9 * 10 ;

		public static explicit operator Angle ( string value )
		{
			if ( value . EndsWith ( "°" ) )
			{
				return new Angle ( Convert . ToDouble ( value . TrimEnd ( '°' ) ) ) ;
			}
			else if ( value . EndsWith ( "ᵍ" ) ||
					value . EndsWith ( "gon" ) )
			{
				return new Angle ( Convert . ToDouble ( value . TrimEnd ( ( "ᵍ" + "gon" ) . ToCharArray ( ) ) ) ) ;
			}
			else
			{
				return new Angle ( Convert . ToDouble ( value ) / Math . PI * 180 ) ;
			}
		}


		public static Angle FromDegree ( double degree ) { return new Angle ( degree ) ; }

		public static Angle FromRedius ( double radius ) { return new Angle ( radius / Math . PI * 180 ) ; }

		public static Angle FromGrad ( double grad ) { return new Angle ( grad / 10 * 9 ) ; }

		public Angle Normal ( ) { return new Angle ( ) ; }

		public override string ToString ( ) { return $"{Degree}°" ; }

	}

}
