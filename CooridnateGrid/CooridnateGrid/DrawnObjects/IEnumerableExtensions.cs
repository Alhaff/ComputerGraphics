using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
	public static class IEnumerableExtensions
	{
		static private bool PointBelongsStraightLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
			double diff = (point.X - lineStart.X) / (lineEnd.X - lineStart.X) -
						  (point.Y - lineStart.Y) / (lineEnd.Y - lineStart.Y);
			return diff > 0 - 1E-10 && diff < 0 + 1E-10;
		}
		public static IEnumerable<Tuple<Vector2, Vector2>> LineCreator(this IEnumerable<Vector2> items)
		{
			var line = new LinkedList<Vector2>();
			foreach (Vector2 item in items)
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
			yield return Tuple.Create(line.First.Value, line.Last.Value);
		}
	}
}
