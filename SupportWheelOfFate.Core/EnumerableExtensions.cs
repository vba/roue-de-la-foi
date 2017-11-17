using System;
using System.Collections.Generic;
using System.Linq;

namespace SupportWheelOfFate.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng = null)
        {
            var elements = source.ToArray();
            var random = rng ?? new Random();
            for (var i = elements.Length - 1; i >= 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}