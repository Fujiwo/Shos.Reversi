using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Shos.Reversi.Core.Test
{
    using Shos.Reversi.Core.Helpers;

    [TestClass]
    public class TwoDimensionalArrayExtensionTest
    {
        [TestMethod]
        public void TableIndexTest()
        {
            for (var row = 0; row < TableIndex.MaximumRowNumber; row++) {
                for (var column = 0; column < TableIndex.MaximumColumnNumber; column++) {
                    var index = new TableIndex { Row = row, Column = column };
                    var temporary = TableIndex.FromLinearIndex(index.LinearIndex);
                    Assert.IsTrue(index.Equals(temporary));
                    Assert.IsTrue(index == temporary);
                    Assert.IsFalse(index != temporary);
                }
            }
        }

        [TestMethod]
        public void TwoDimensionalArrayTest()
        {
            TableIndex.MaximumRowNumber    = 2;
            TableIndex.MaximumColumnNumber = 3;

            var array = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            Assert.AreEqual(1, array[0, 0]);
            Assert.AreEqual(2, array[0, 1]);
            Assert.AreEqual(3, array[0, 2]);
            Assert.AreEqual(4, array[1, 0]);
            Assert.AreEqual(5, array[1, 1]);
            Assert.AreEqual(6, array[1, 2]);

            Assert.AreEqual(2, array.GetLength(0));
            Assert.AreEqual(3, array.GetLength(1));

            var arrayLength = array.Length;
            Assert.AreEqual(3 * 2, arrayLength);

            var allIndexes = array.AllIndexes().ToList();
            Assert.AreEqual(arrayLength, allIndexes.Count);

            for (var linearIndex = 0; linearIndex < allIndexes.Count; linearIndex++) {
                Assert.AreEqual(TableIndex.FromLinearIndex(linearIndex), allIndexes[linearIndex]);
            }

            Assert.AreEqual(1, array.Get(allIndexes[0]));
            Assert.AreEqual(2, array.Get(allIndexes[1]));
            Assert.AreEqual(3, array.Get(allIndexes[2]));
            Assert.AreEqual(4, array.Get(allIndexes[3]));
            Assert.AreEqual(5, array.Get(allIndexes[4]));
            Assert.AreEqual(6, array.Get(allIndexes[5]));
        }
    }
}
