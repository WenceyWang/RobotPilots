using System;
using System.Collections.Generic;
using System.Text;

using JetBrains . Annotations ;

using RobotPilots . Vision . Managed . Utility ;

namespace RobotPilots.Vision.Managed.Dispatch
{
    public class Dispatcher:NeedRegisBase<DispatcherType, DispatcherAttribute, Dispatcher>
    {
		[Startup]
		public static void LoadDispatcher()
		{
			LoadAll();
		}

	}

	public class DispatcherAttribute : NeedRegisBase . NeedRegisAttributeBase
	{

	}

	public class DispatcherType:RegisType<DispatcherType, DispatcherAttribute, Dispatcher>
	{

		public DispatcherType ( [NotNull] Type entryType ) : base ( entryType )
		{
		}

	}

}
