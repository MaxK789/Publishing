using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.DTOs;
using Publishing.Core.Services;
using FluentValidation;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class OrderInputValidatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Validate_InvalidDto_Throws()
        {
            var validator = new OrderInputValidator();
            var dto = new CreateOrderDto { Pages = -1 };
            validator.Validate(dto);
        }

        [TestMethod]
        public void Validate_ValidDto_DoesNotThrow()
        {
            var validator = new OrderInputValidator();
            var dto = new CreateOrderDto { Type = "book", Name = "n", Pages = 1, Tirage = 1, Printery = "p", PersonId = "1" };
            validator.Validate(dto);
        }
    }
}
