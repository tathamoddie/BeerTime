using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerTime.WebUI
{
    static class EnumerableExtensions
    {
        internal static T Random<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();
            var maxIndex = sourceArray.Length - 1;
            var randomIndex = (int)Math.Round(new Random().NextDouble() * maxIndex, 0);
            return sourceArray[randomIndex];
        }
    }
}