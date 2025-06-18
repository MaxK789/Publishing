using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class PriceCalculatorTests
    {
        private PriceCalculator _calculator = null!;

        [TestInitialize]
        public void Init()
        {
            _calculator = new PriceCalculator();
        }

        [DataTestMethod]
        [DataRow(10, 3, 75.0)]
        [DataRow(0, 5, 0.0)]
        [DataRow(5, 0, 0.0)]
        public void CalculateTotal_ReturnsExpected(int pages, int copies, double expectedD)
        {
            var expected = (decimal)expectedD;
            var result = _calculator.Calculate(pages, copies, 2.5m);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CalculateTotal_ZeroPricePerPage_ReturnsZero()
        {
            var result = _calculator.Calculate(10, 2, 0m);
            Assert.AreEqual(0m, result);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CalculateTotal_LargeValues_Overflow()
        {
            _calculator.Calculate(int.MaxValue, int.MaxValue, decimal.MaxValue);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTotal_Negative_Throws()
        {
            _calculator.Calculate(-1, 1, 2.5m);
        }
    }
}
