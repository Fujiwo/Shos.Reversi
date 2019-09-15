using System;
using System.Collections.Generic;
using System.Linq;

namespace Shos.Reversi.Core.Helpers
{
    public static class TwoDimensionalArrayExtension
    {
        public static IEnumerable<TableIndex> AllIndexes<T>(this T[,] @this)
            => from row    in Enumerable.Range(0, @this.GetLength(0))
               from column in Enumerable.Range(0, @this.GetLength(1))
               select new TableIndex { Row = row, Column = column };

        public static IEnumerable<(TableIndex, T)> ToSequenceWithIndex<T>(this T[,] @this)
            => from row    in Enumerable.Range(0, @this.GetLength(0))
               from column in Enumerable.Range(0, @this.GetLength(1))
               select (new TableIndex { Row = row, Column = column }, @this[row, column]);

        public static IEnumerable<T> ToSequence<T>(this T[,] @this)
            => from row    in Enumerable.Range(0, @this.GetLength(0))
               from column in Enumerable.Range(0, @this.GetLength(1))
               select @this[row, column];

        public static T Get<T>(this T[,] @this, TableIndex index)
            => @this[index.Row, index.Column];

        public static void Set<T>(this T[,] @this, TableIndex index, T value)
            => @this[index.Row, index.Column] = value;

        public static void ForEachWithIndex<T>(this T[,] @this, Action<TableIndex, T> action)
            => @this.AllIndexes().ForEach(index => action(index, @this.Get(index)));

        public static void ForEach<T>(this T[,] @this, Action<T> action)
            => @this.AllIndexes().ForEach(index => action(@this.Get(index)));

        public static int Count<T>(this T[,] @this, Func<T, bool> isMatch)
            => @this.ToSequence().Count(isMatch);
    }
}
