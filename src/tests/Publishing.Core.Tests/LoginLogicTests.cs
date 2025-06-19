using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCrypt.Net;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class LoginLogicTests
    {
        [TestMethod]
        public void CorrectPassword_Verifies()
        {
            string password = "secret";
            string hash = BCrypt.Net.BCrypt.HashPassword(password, 11);
            bool ok = BCrypt.Net.BCrypt.Verify(password, hash);
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void IncorrectPassword_FailsVerify()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("secret", 11);
            bool ok = BCrypt.Net.BCrypt.Verify("wrong", hash);
            Assert.IsFalse(ok);
        }
    }
}
