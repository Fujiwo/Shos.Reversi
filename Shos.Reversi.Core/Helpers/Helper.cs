using MathNet.Numerics.Random;
using System.Collections.Generic;

namespace Shos.Reversi.Core.Helpers
{
    public static class HelperExtensions
    {
        static MersenneTwister random = new MersenneTwister();

        public static void Shuffle<TElement>(this TElement[] @this)
        {
            for (var count = @this.Length; count > 1; count--)
                ShuffleTail(@this, count);
        }

        static void ShuffleTail<TElement>(IList<TElement> collection, int count)
        {
            var lastIndex   = count - 1         ;
            var randomIndex = random.Next(count);
            if (lastIndex != randomIndex)
                (collection[lastIndex], collection[randomIndex]) = (collection[randomIndex], collection[lastIndex]);
        }
    }
}
