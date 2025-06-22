using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class UserSessionTests
    {
        [TestMethod]
        public void Properties_SetAndClear_WorkCorrectly()
        {
            var session = new UserSession();
            session.UserId = "42";
            session.UserName = "Alice";
            session.UserType = "admin";
            session.Token = "t";

            Assert.AreEqual("42", session.UserId);
            Assert.AreEqual("Alice", session.UserName);
            Assert.AreEqual("admin", session.UserType);
            Assert.AreEqual("t", session.Token);

            session.UserId = string.Empty;
            session.UserName = string.Empty;
            session.UserType = string.Empty;
            session.Token = string.Empty;

            Assert.AreEqual(string.Empty, session.UserId);
            Assert.AreEqual(string.Empty, session.UserName);
            Assert.AreEqual(string.Empty, session.UserType);
            Assert.AreEqual(string.Empty, session.Token);
        }
    }
}
