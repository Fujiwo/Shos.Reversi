using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shos.Reversi.Core.Test
{
    using Shos.Reversi.Core.Helpers;

    [TestClass]
    public class HelperTest
    {
        [TestMethod]
        public void ShuffleTest()
        {
            const int testCount = 1000;
            var counter1 = 0;
            var counter2 = 0;
            var array = new [] { 1, 2 };
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
    }
}
