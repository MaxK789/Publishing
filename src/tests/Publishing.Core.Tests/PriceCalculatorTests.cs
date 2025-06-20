using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class PriceCalculatorTests
    {
        private PriceCalculator _calculator = null!;

        [TestInitialize]
        public void Init()
        {
            _calculator = new PriceCalculator(new StandardDiscountPolicy());
        }

        [DataTestMethod]
        [DataRow(10, 3, 2.5, 75.0)]
        [DataRow(1, 1, 1.0, 1.0)]
        public void CalculateTotal_ReturnsExpected(int pages, int copies, double price, double expectedD)
        {
            var expected = (decimal)expectedD;
            var result = _calculator.Calculate(pages, copies, (decimal)price);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow(0, 5, 2.5)]
        [DataRow(5, 0, 2.5)]
        [DataRow(5, 5, 0.0)]
        public void CalculateTotal_ZeroParameter_ReturnsZero(int pages, int copies, double price)
        {
            var result = _calculator.Calculate(pages, copies, (decimal)price);
            Assert.AreEqual(0m, result);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void CalculateTotal_LargeValues_Overflow()
        {
            _calculator.Calculate(int.MaxValue, int.MaxValue, decimal.MaxValue);
        }

        [DataTestMethod]
        [DataRow(-1, 1, 1.0)]
        [DataRow(1, -1, 1.0)]
        [DataRow(1, 1, -1.0)]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTotal_Negative_Throws(int pages, int copies, double price)
        {
            _calculator.Calculate(pages, copies, (decimal)price);
        }
    }
}
