using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CooridnateGrid.DrawnObjects
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			T temp = default(T);
			int counter = 0;
			foreach (T item in items)
			{
				counter++;
				if (counter == 2)
				{
					yield return Tuple.Create(temp, item);
					counter = 1;
					temp = item;
				}
				else
				{
					temp = item;
				}
			}
		}
	}
}
