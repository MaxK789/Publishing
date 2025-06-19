using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class StatusUpdateTests
    {
        [TestMethod]
        public void UpdateQuery_IsCorrect()
        {
            string expected = "UPDATE Orders SET statusOrder = 'завершено' WHERE statusOrder <> 'завершено' AND dateFinish < GETDATE()";
            string actual = "UPDATE Orders SET statusOrder = 'завершено' WHERE statusOrder <> 'завершено' AND dateFinish < GETDATE()";
            Assert.AreEqual(expected, actual);
        }
    }
}
