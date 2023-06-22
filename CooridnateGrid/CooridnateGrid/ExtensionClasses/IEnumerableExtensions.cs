using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.ExtensionsClasses
{
	public static class IEnumerableExtensions
	{
		static private bool PointBelongsStraightLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
			double diff = (point.X - lineStart.X) / (lineEnd.X - lineStart.X) -
						  (point.Y - lineStart.Y) / (lineEnd.Y - lineStart.Y);
			return diff > 0 - 1E-10 && diff < 0 + 1E-10;
		}
		public static IEnumerable<Tuple<Vector3, Vector3>> LineCreator(this IEnumerable<Vector3> items)
		{
			var line = new LinkedList<Vector3>();
			foreach (Vector3 item in items)
			{
				if (line.Count == 2)
				{
					if(PointBelongsStraightLine(line.First.Value,line.Last.Value,item))
					{
						line.RemoveLast();
					}
                    else
                    {
						yield return Tuple.Create(line.First.Value, line.Last.Value);
						line.RemoveFirst();
					}
				}
				line.AddLast(item);
			}
			if(line.Count > 0)
				yield return Tuple.Create(line.First.Value, line.Last.Value);
		}
	}
}
