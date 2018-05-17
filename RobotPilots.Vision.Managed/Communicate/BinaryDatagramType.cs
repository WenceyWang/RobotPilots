using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public enum BinaryDatagramType : byte
	{

		//Send Package 0-127,

		//Cradle Hand

		TargetPosition = 0 ,

		TargetAngle = 1 ,

		TargetDeltaAngle = 2 ,

		Fire = 3 ,

		FrictionSpeed = 4 ,

        PwmValue=5,

		SupplyStationStatus=6,


		//Receive Package 128-255

		GimbalPosition = 128 ,

		GimbalTarget = 129 ,

		GimbalVelocity = 130 ,


		//Chassis

		FrictionWheelVelocity = 131,



		//todo:Chassis speed

		//todo: how many bullet remain


		UpdateSupplyStationStatus=140,

	}

}
