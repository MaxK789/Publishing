using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Infrastructure.DataAccess;
using Publishing.Core.Domain;
using System.Data;

namespace Publishing.Core.Tests;

[TestClass]
public class SqlQueryTests
{
    [TestMethod]
    public void GetActiveOrdersQuery_HasExpectedSql()
    {
        var query = new GetActiveOrdersQuery();
        StringAssert.Contains(query.Sql, "SELECT");
    }
}
