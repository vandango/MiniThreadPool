using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
	public static class ListExtensions
	{
		public static T Grap<T>(this List<T> instance, int index)
		{
			try
			{
				if(index < instance.Count)
				{
					T item = instance[index];

					instance.Remove(item);

					return item;
				}
				else
				{
					return default(T);
				}
			}
			catch
			{
				throw new IndexOutOfRangeException(string.Format(
					"The index {0} is out of range.",
					index
				));
			}
		}
	}
}
