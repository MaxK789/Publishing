using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;
using Publishing.AppLayer.Validators;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class EmailValidatorTests
    {
        private readonly IValidator<string> _validator = new EmailValidator();

        [DataTestMethod]
        [DataRow("test@example.com", true)]
        [DataRow("user.name@domain.co", true)]
        [DataRow("invalid_email", false)]
        [DataRow("another@invalid", false)]
        public void RegistrationForm_EmailValidation(string email, bool expected)
        {
            bool result = _validator.Validate(email).IsValid;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("org@example.com", true)]
        [DataRow("wrong@address", false)]
        public void OrganizationForm_EmailValidation(string email, bool expected)
        {
            bool result = _validator.Validate(email).IsValid;
            Assert.AreEqual(expected, result);
        }
    }
}
