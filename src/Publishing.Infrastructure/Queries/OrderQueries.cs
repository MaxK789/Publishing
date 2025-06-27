using System.Data;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IDbHelper _helper;

        public OrderQueries(IDbHelper helper)
        {
            _helper = helper;
        }

        public Task<DataTable> GetActiveAsync()
        {
            const string sql = @"SELECT O.namePrintery, Prod.typeProduct, Prod.nameProduct, Per.FName, Per.LName, O.dateOrder, O.dateStart, O.dateFinish, O.statusOrder, O.price FROM(Orders O INNER JOIN Product Prod ON Prod.idProduct = O.idProduct ) INNER JOIN Person Per ON Per.idPerson = Prod.idPerson WHERE O.statusOrder IN ('в роботі', 'InProgress') ORDER BY O.dateOrder";
            return _helper.QueryDataTableAsync(sql);
        }

        public Task<DataTable> GetByPersonAsync(string personId)
        {
            const string sql = "SELECT * FROM Orders where idPerson = @id";
            return _helper.QueryDataTableAsync(sql, new { id = personId });
        }

        public Task<DataTable> GetAllAsync()
        {
            return _helper.QueryDataTableAsync("SELECT * FROM Orders");
        }
    }
}
