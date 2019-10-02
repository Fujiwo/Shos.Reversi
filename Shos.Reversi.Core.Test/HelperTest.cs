using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shos.Reversi.Core.Test
{
    using Shos.Reversi.Core.Helpers;
    using System;

    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void ShuffleTest()
        {
            const int testCount = 1000;
            var counter1 = 0;
            var counter2 = 0;
            var array = new[] { 1, 2 };
            for (var counter = 0; counter < testCount; counter++) {
                array.Shuffle();
                if (array[0] == 1 && array[1] == 2)
                    counter1++;
                else if (array[0] == 2 && array[1] == 1)
                    counter2++;
            }
            Assert.AreEqual(testCount, counter1 + counter2);
            Assert.IsTrue((int)(testCount * 0.4) <= counter1);
            Assert.IsTrue((int)(testCount * 0.6) >= counter1);
        }

        //[TestMethod]
        //public void MersenneTwisterNextUIntTest()
        //{
        //    const uint testCount = 1000U;

        //    var random = new MersenneTwister();

        //    for (var counter = 0U; counter < testCount; counter++) {
        //        for (var c = 0U; c < testCount; c++) {
        //            var value = random.NextUInt();
        //            Assert.IsTrue(0 <= value);
        //            Assert.IsTrue(value <= uint.MaxValue);
        //        }
        //    }
        //}

        //[TestMethod]
        //public void MersenneTwisterNextUIntWithMinimumAndMaximumTest()
        //{
        //    const uint testRange = 100U;
        //    const uint testCount = 1000U;

        //    var random = new MersenneTwister();

        //    for (var counter = 0U; counter < testCount; counter++) {
        //        var minimum = random.NextUInt(testRange);
        //        var maximum = random.NextUInt(testRange);
        //        Sort(ref minimum, ref maximum);
        //        if (minimum == maximum)
        //            maximum++;

        //        for (var c = 0U; c < testCount; c++) {
        //            var value = random.NextUInt(minimum, maximum);
        //            Assert.IsTrue(minimum <= value);
        //            Assert.IsTrue(value < maximum);
        //        }
        //    }
        //}

        [TestMethod]
        public void MersenneTwisterNextUIntWithMaximumTest()
        {
            const uint testRange = 100U;
            const uint testCount = 1000U;

            var random = new MersenneTwister();

            for (var counter = 0U; counter < testCount; counter++) {
                var maximum = random.NextUInt(testRange - 1) + 1;

                for (var c = 0U; c < testCount; c++) {
                    var value = random.NextUInt(maximum);
                    Assert.IsTrue(0 <= value);
                    Assert.IsTrue(value < maximum);
                }
            }
        }

        [TestMethod]
        public void MersenneTwisterNextWithMinimumAndMaximumTest()
        {
            const int testRange =  100;
            const uint testCount = 1000U;

            var random = new MersenneTwister();

            for (var counter = 0U; counter < testCount; counter++) {
                var minimum = random.Next(-testRange, testRange);
                var maximum = random.Next(-testRange, testRange);
                Sort(ref minimum, ref maximum);
                if (minimum == maximum)
                    maximum++;

                for (var c = 0U; c < testCount; c++) {
                    var value = random.Next(minimum, maximum);
                    Assert.IsTrue(minimum <= value);
                    Assert.IsTrue(value < maximum);
                }
            }
        }

        [TestMethod]
        public void MersenneTwisterNextWithMaximumTest()
        {
            const int testRange = 100;
            const uint testCount = 1000U;

            var random = new MersenneTwister();

            for (var counter = 0U; counter < testCount; counter++) {
                var maximum = random.Next(testRange - 1) + 1;
                for (var c = 0U; c < testCount; c++) {
                    var value = random.Next(maximum);
                    Assert.IsTrue(0 <= value);
                    Assert.IsTrue(value < maximum);
                }
            }
        }

        [TestMethod]
        public void MersenneTwisterNextTest()
        {
            const int testRange = 10;
            const uint testCount = 1000000000U;

            var random = new MersenneTwister();
            var counts = new uint[testRange];
            for (var counter = 0U; counter < testCount; counter++) {
                var value = random.Next(0, testRange);
                Assert.IsTrue(0 <= value && value < testRange);
                counts[value]++;
                value = random.Next(testRange);
                Assert.IsTrue(0 <= value && value < testRange);
                counts[value]++;
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void MersenneTwisterNextUTest()
        {
            const uint testRange = 10U;
            const uint testCount = 1000000000U;

            var random = new MersenneTwister();
            var counts = new uint[testRange];
            for (var counter = 0U; counter < testCount; counter++) {
                //var value = random.NextUInt(0, testRange);
                //Assert.IsTrue(0U <= value && value < testRange);
                //counts[value]++;
                var value = random.NextUInt(testRange);
                Assert.IsTrue(0U <= value && value < testRange);
                counts[value]++;
            }
            Assert.IsTrue(true);
        }

        static void Sort<T>(ref T value1, ref T value2) where T : IComparable<T>
        {
            if (value1.CompareTo(value2) > 0)
                (value1, value2) = (value2, value1);
        }
    }
}
