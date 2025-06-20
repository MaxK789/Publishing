using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.AppLayer.Validators;
using FluentValidation;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class EmailValidatorTests
    {
        [DataTestMethod]
        [DataRow("test@example.com", true)]
        [DataRow("user.name@domain.co", true)]
        [DataRow("invalid_email", false)]
        [DataRow("another@invalid", false)]
        public void RegistrationForm_EmailValidation(string email, bool expected)
        {
            var validator = new EmailValidator();
            bool result = validator.Validate(email).IsValid;
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("org@example.com", true)]
        [DataRow("wrong@address", false)]
        public void OrganizationForm_EmailValidation(string email, bool expected)
        {
            var validator = new EmailValidator();
            bool result = validator.Validate(email).IsValid;
            Assert.AreEqual(expected, result);
        }
    }
}
