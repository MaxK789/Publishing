using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using Publishing.AppLayer.Validators;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class PhoneFaxValidatorTests
    {
        private readonly IValidator<string> _validator = new PhoneFaxValidator();

        [DataTestMethod]
        [DataRow("+1234567890", true)]
        [DataRow("1234567", true)]
        [DataRow("+1(234)567", false)]
        [DataRow("abc", false)]
        public void ValidatePhoneFax(string input, bool expected)
        {
            bool result = _validator.Validate(input).IsValid;
            Assert.AreEqual(expected, result);
        }
    }
}
