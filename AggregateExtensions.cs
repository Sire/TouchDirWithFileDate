using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace TouchDirWithFileDate
{
	static class AggregateExtensions
	{

        public static TSource Median<TSource>(this IEnumerable<TSource> source)
            where TSource : struct, INumber<TSource>
            => Median<TSource, TSource>(source);

        public static TResult Median<TSource, TResult>(this IEnumerable<TSource> source)
            where TSource : struct, INumber<TSource>
            where TResult : struct, INumber<TResult>
        {
            var array = source.ToArray();
            var length = array.Length;
            if (length == 0)
            {
                throw new InvalidOperationException("Sequence contains no elements.");
            }
            Array.Sort(array);
            var index = length / 2;
            var value = TResult.CreateChecked(array[index]);
            if (length % 2 == 1)
            {
                return value;
            }
            var sum = value + TResult.CreateChecked(array[index - 1]);
            return sum / TResult.CreateChecked(2);
        }

	}
}
