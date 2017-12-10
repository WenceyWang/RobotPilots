using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public enum BinaryDatagramType : byte
	{

		//Send Package 0-127,

		TargetPosition = 0 ,

		TargetAngle = 1 ,

		TargetDeltaAngle = 2 ,

		//Receive Package 128-255

		CradleHeadPosition = 128 ,

		CradleHeadVelocity = 129 ,

		//Chassis

		FrictionWheelVelocity = 130

		//todo:Chassis speed

		//todo: how many bullet remain

	}

}
