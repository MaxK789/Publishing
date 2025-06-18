using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ParsingTests
    {
        [DataTestMethod]
        [DataRow("10", true)]
        [DataRow("abc", false)]
        public void PageNumberParsing(string value, bool expected)
        {
            bool result = int.TryParse(value, out _);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("5", true)]
        [DataRow("notint", false)]
        public void TirageParsing(string value, bool expected)
        {
            bool result = int.TryParse(value, out _);
            Assert.AreEqual(expected, result);
        }
    }
}
