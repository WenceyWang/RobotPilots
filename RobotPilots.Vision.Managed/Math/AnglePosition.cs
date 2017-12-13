using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Math
{

	public struct AnglePosition : IEquatable <AnglePosition>
	{

		public AnglePosition ( Angle xYaw , Angle yPitch )
		{
			XYaw = xYaw ;
			YPitch = yPitch ;
		}

		public static AnglePosition operator - ( AnglePosition left , AnglePosition right )
		{
			return new AnglePosition ( left . XYaw - right . XYaw , left . YPitch - right . YPitch ) ;
		}

		public static AnglePosition operator + ( AnglePosition left , AnglePosition right )
		{
			return new AnglePosition ( left . XYaw + right . XYaw , left . YPitch + right . YPitch ) ;
		}

		public bool Equals ( AnglePosition other )
		{
			return XYaw . Equals ( other . XYaw ) && YPitch . Equals ( other . YPitch ) ;
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
			{
				return false ;
			}

			return obj is AnglePosition && Equals ( ( AnglePosition ) obj ) ;
		}

		public override int GetHashCode ( )
		{
			unchecked
			{
				return ( XYaw . GetHashCode ( ) * 397 ) ^ YPitch . GetHashCode ( ) ;
			}
		}

		public static bool operator == ( AnglePosition left , AnglePosition right ) { return left . Equals ( right ) ; }

		public static bool operator != ( AnglePosition left , AnglePosition right ) { return ! left . Equals ( right ) ; }

		public Angle XYaw { get ; }

		public Angle YPitch { get ; }

	}

}
