using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesStore.MyExtensions
{
    public static partial class MyExtensions
    {
        public static bool IsLast<T>(this IEnumerable<T> items, T item)
        {
            var last = items.LastOrDefault();
            if (last == null)
                return false;
            return item.Equals(last); // OR Object.ReferenceEquals(last, item)
        }

        public static bool IsFirst<T>(this IEnumerable<T> items, T item)
        {
            var first = items.FirstOrDefault();
            if (first == null)
                return false;
            return item.Equals(first);
        }

        public static bool IsFirstOrLast<T>(this IEnumerable<T> items, T item)
        {
            return items.IsFirst(item) || items.IsLast(item);
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }
}