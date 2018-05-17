using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using Microsoft . VisualStudio . TestTools . UnitTesting ;
using RobotPilots.Vision.Managed.Utility;

namespace RobotPilots . Vision . Managed . Tests
{

	[TestClass]
	public class StartUpTest
	{

		[TestMethod]
		public void RunTaskTest()
		{
			Startup.RunAllTask().Wait();
		}

	}

	[TestClass]
	public class UnitTest1
	{

		[TestMethod]
		public void TestMethod1 ( )
		{
		}

	}

}
