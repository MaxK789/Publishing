using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class QueryBuilderTests
    {
        private string BuildProfileQuery(Dictionary<string, string> values)
        {
            string query = "UPDATE Person SET";
            if (values.ContainsKey("FName")) query += " FName = @FName,";
            if (values.ContainsKey("LName")) query += " LName = @LName,";
            if (values.ContainsKey("Email")) query += " emailPerson = @Email,";
            if (values.ContainsKey("Status")) query += " typePerson = @Status,";
            if (values.ContainsKey("phone")) query += " phonePerson = @phone,";
            if (values.ContainsKey("fax")) query += " faxPerson = @fax,";
            if (values.ContainsKey("address")) query += " addressPerson = @address,";
            if (query.EndsWith(",")) query = query.Remove(query.Length - 1, 1);
            query += " WHERE idPerson = @id";
            return query;
        }

        [TestMethod]
        public void ProfileForm_QueryStartsWithUpdate()
        {
            var values = new Dictionary<string, string> { { "FName", "John" } };
            string query = BuildProfileQuery(values);
            StringAssert.StartsWith(query, "UPDATE Person SET");
        }

        [TestMethod]
        public void ProfileForm_ParameterCountMatches()
        {
            var values = new Dictionary<string, string>
            {
                {"FName", "John"},
                {"LName", "Doe"},
                {"Email", "john@d.com"},
                {"Status", "s"}
            };
            string query = BuildProfileQuery(values);
            int parameterCount = Regex.Matches(query, "@").Count;
            Assert.AreEqual(values.Count + 1, parameterCount);
        }

        private string BuildOrganizationQuery(Dictionary<string, string> values, bool isInsert)
        {
            if (isInsert)
            {
                return "INSERT INTO Organization(nameOrganization, emailOrganization, phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES (@orgName, @Email, @phone, @fax, @address, @id)";
            }
            string query = "UPDATE Organization SET";
            if (values.ContainsKey("orgName")) query += " nameOrganization = @orgName,";
            if (values.ContainsKey("Email")) query += " emailOrganization = @Email,";
            if (values.ContainsKey("phone")) query += " phoneOrganization = @phone,";
            if (values.ContainsKey("fax")) query += " faxOrganization = @fax,";
            if (values.ContainsKey("address")) query += " addressOrganization = @address,";
            if (query.EndsWith(",")) query = query.Remove(query.Length - 1, 1);
            query += " WHERE idPerson = @id";
            return query;
        }

        [TestMethod]
        public void OrganizationForm_RemoveTrailingComma()
        {
            var values = new Dictionary<string, string> { { "orgName", "Org" }, { "Email", "e@e.com" } };
            string query = BuildOrganizationQuery(values, false);
            Assert.IsFalse(query.EndsWith(","));
            StringAssert.EndsWith(query, "WHERE idPerson = @id");
        }

        [TestMethod]
        public void OrganizationForm_InsertQueryCorrect()
        {
            var values = new Dictionary<string, string>();
            string query = BuildOrganizationQuery(values, true);
            string expected = "INSERT INTO Organization(nameOrganization, emailOrganization, phoneOrganization, faxOrganization, addressOrganization, idPerson) VALUES(@orgName, @Email, @phone, @fax, @address, @id)";
            Assert.AreEqual(expected, query);
        }
    }
}
