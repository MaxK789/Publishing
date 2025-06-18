using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class PriceCalculatorTests
    {
        [TestInitialize]
        public void Init()
        {
            // reset to default price before each test to avoid cross-test contamination
            PriceCalculator.PricePerPage = 2.5m;
        }

        [DataTestMethod]
        [DataRow(10, 3, 75.0)]
        [DataRow(0, 5, 0.0)]
        [DataRow(5, 0, 0.0)]
        public void CalculateTotal_ReturnsExpected(int pages, int copies, double expectedD)
        {
            var expected = (decimal)expectedD;
            var result = PriceCalculator.CalculateTotal(pages, copies);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateTotal_ZeroPricePerPage_ReturnsZero()
        {
            PriceCalculator.PricePerPage = 0m;
            var result = PriceCalculator.CalculateTotal(10, 2);
            Assert.AreEqual(0m, result);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CalculateTotal_LargeValues_Overflow()
        {
            PriceCalculator.PricePerPage = decimal.MaxValue;
            PriceCalculator.CalculateTotal(int.MaxValue, int.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTotal_Negative_Throws()
        {
            PriceCalculator.CalculateTotal(-1, 1);
        }
    }
}
