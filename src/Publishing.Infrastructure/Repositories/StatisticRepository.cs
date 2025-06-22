using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private readonly IDbHelper _helper;

        public StatisticRepository(IDbHelper helper)
        {
            _helper = helper;
        }

        public Task<List<string[]>> GetAuthorNamesAsync()
        {
            const string sql = "SELECT DISTINCT (FName + ' ' + LName) AS Author FROM Person P INNER JOIN Orders O ON O.idPerson = P.idPerson";
            return _helper.QueryStringListAsync(sql);
        }

        public Task<List<string[]>> GetOrdersPerMonthAsync()
        {
            const string sql = "SELECT DATENAME(MONTH, dateOrder) AS orderMonth, COUNT(*) AS Number FROM Orders WHERE YEAR(dateOrder) = YEAR(GETDATE()) GROUP BY DATENAME(MONTH, dateOrder)";
            return _helper.QueryStringListAsync(sql);
        }

        public Task<List<string[]>> GetOrdersPerAuthorAsync()
        {
            const string sql = "SELECT (P.FName + ' ' + P.LName) AS Author, COUNT(*) AS Number FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson GROUP BY (P.FName + ' ' + P.LName)";
            return _helper.QueryStringListAsync(sql);
        }

        public Task<List<string[]>> GetOrdersPerAuthorAsync(string fullName)
        {
            const string sql = "SELECT (P.FName + ' ' + P.LName) AS Author, COUNT(*) AS Number FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson WHERE (P.FName + ' ' + P.LName) = @fullNameAuthor GROUP BY (P.FName + ' ' + P.LName)";
            return _helper.QueryStringListAsync(sql, new { fullNameAuthor = fullName });
        }

        public Task<List<string[]>> GetOrdersPerMonthAsync(DateTime start, DateTime end)
        {
            const string sql = "SELECT DATENAME(MONTH, dateOrder) AS orderMonth, COUNT(*) AS Number FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson WHERE dateOrder BETWEEN @StartDate AND @EndDate GROUP BY DATENAME(MONTH, dateOrder)";
            return _helper.QueryStringListAsync(sql, new { StartDate = start, EndDate = end });
        }

        public Task<List<string[]>> GetTiragePerAuthorAsync()
        {
            const string sql = "SELECT (P.FName + ' ' + P.LName) AS Author, Sum(tirage) AS sumTirage FROM Orders O INNER JOIN Person P ON P.idPerson = O.idPerson GROUP BY (P.FName + ' ' + P.LName)";
            return _helper.QueryStringListAsync(sql);
        }
    }
}
