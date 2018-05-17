using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

using OpenCvSharp ;

namespace RobotPilots . Vision . Managed .Utility
{

	[PublicAPI]
	public class LinearInterpolationLookupTable : ISelfSerializeable
	{

		public LinearInterpolationLookupTable()
		{
			PointsList = new List<Point>();
			Points = new ReadOnlyCollection<Point>(PointsList);
		}

		public LinearInterpolationLookupTable(XElement element)
		{

		}

		public void AddPoint(Point point)
		{
			PointsList.Sort((left, right) => left.X - right.X);
		}

		public ReadOnlyCollection<Point> Points { get; }

		private List<Point> PointsList { get; }

		public double this[double x] => Find(x);

		public double Find(double x)
		{
			for (int i = 0; i < Points.Count - 1; i++)
			{
				if (Points[i].X <= x && x < Points[i + 1].X)
				{
					return ((Points[i + 1].Y - Points[i].Y) * (x - Points[i].X) /
							(Points[i + 1].X - Points[i].X)) + Points[i].Y;
				}
			}

			return 0;
		}

		public XElement ToXElement()
		{
			XElement result = new XElement(nameof(LinearInterpolationLookupTable));
			foreach (Point point in Points)
			{
				XElement pointElement = new XElement(nameof(Point));

				pointElement.SetAttributeValue(nameof(Point.X), point.X);
				pointElement.SetAttributeValue(nameof(Point.Y), point.Y);

				result.Add(pointElement);
			}


			return result;
		}

	}

}