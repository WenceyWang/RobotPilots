using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using RobotPilots . Vision . Managed . Utility ;
using RobotPilots . Vision . Managed . Visual . ImagePreprocessors ;

namespace RobotPilots . Vision . Managed . Visual
{

	public class ImagePipeline
	{
		public ICamera Source { get; }


		public CompositeImagePreprocessor Preprocessor { get; }



	}

	[Module]
	public class VisualModule:IModule
	{

		public List<ICamera> Cameras { get; }

		public List<ImagePipeline> PipeLines { get; }

		public static VisualModule Current { get; private set ; }

		public string [ ] Dependencies { get ; } = { } ;

		public void Prepare(Configurations configuration)
		{
			Current = this;

			

		}

		public void Dispose() { }

	}

}