using System;
using System.Collections.Generic;

namespace Shos.Reversi.Core.Helpers
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action action)
        {
            foreach (var item in @this)
                action();
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this)
                action(item);
        }
    }
}
