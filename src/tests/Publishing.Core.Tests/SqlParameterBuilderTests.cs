using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Core.Services;
using System.Collections.Generic;
using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class SqlParameterBuilderTests
    {
        [TestMethod]
        public void FromDictionary_BuildsParameters()
        {
            var values = new Dictionary<string, object?>
            {
                ["Name"] = "John",
                ["Age"] = 30
            };

            var list = SqlParameterBuilder.FromDictionary(values);

            Assert.AreEqual(values.Count, list.Count);
            CollectionAssert.AreEqual(new[] {"@Name", "@Age"}, list.Select(p => p.ParameterName).ToArray());
        }

        [TestMethod]
        public void FromDictionary_EmptyDictionary_ReturnsEmptyList()
        {
            var values = new Dictionary<string, object?>();
            var list = SqlParameterBuilder.FromDictionary(values);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void FromDictionary_NullValue_UsesDBNull()
        {
            var values = new Dictionary<string, object?>
            {
                ["Phone"] = null
            };
            var list = SqlParameterBuilder.FromDictionary(values);
            Assert.AreEqual(DBNull.Value, list[0].Value);
        }
    }
}
