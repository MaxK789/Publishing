using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class EmailValidatorTests
    {
        private const string Pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        [DataTestMethod]
        [DataRow("test@example.com", true)]
        [DataRow("user.name@domain.co", true)]
        [DataRow("invalid_email", false)]
        [DataRow("another@invalid", false)]
        public void RegistrationForm_EmailValidation(string email, bool expected)
        {
            bool result = Regex.IsMatch(email, Pattern);
            Assert.AreEqual(expected, result);
        }

        [DataTestMethod]
        [DataRow("org@example.com", true)]
        [DataRow("wrong@address", false)]
        public void OrganizationForm_EmailValidation(string email, bool expected)
        {
            bool result = Regex.IsMatch(email, Pattern);
            Assert.AreEqual(expected, result);
        }
    }
}
