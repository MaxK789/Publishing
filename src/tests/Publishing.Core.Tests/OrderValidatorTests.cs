using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.DTOs;
using Publishing.Core.Services;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class OrderValidatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_NullDto_Throws()
        {
            var validator = new OrderValidator();
            validator.Validate(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_NegativePages_Throws()
        {
            var validator = new OrderValidator();
            validator.Validate(new CreateOrderDto { Pages = -1, Tirage = 1 });
        }

        [TestMethod]
        public void Validate_ValidDto_DoesNotThrow()
        {
            var validator = new OrderValidator();
            validator.Validate(new CreateOrderDto { Pages = 1, Tirage = 1 });
        }
    }
}
