using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;
using Dapper;

namespace Publishing.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository, IOrderQueries
    {
        private readonly IDbContext _db;
        private readonly IDbHelper _helper;
        private readonly IUnitOfWork? _uow;

        public OrderRepository(IDbContext db, IDbHelper helper, IUnitOfWork? uow = null)
        {
            _db = db;
            _helper = helper;
            _uow = uow;
        }

        public async Task<int> SaveAsync(Publishing.Core.Domain.Order order)
        {
            const string selectProduct = @"SELECT idProduct FROM Product WHERE typeProduct = @Type AND nameProduct = @Name AND idPerson = @PersonId AND pagesNum = @Pages";
            IEnumerable<int> ids;
            if (_uow != null)
            {
                ids = await _uow.Connection.QueryAsync<int>(selectProduct, new
                {
                    order.Type,
                    order.Name,
                    order.PersonId,
                    order.Pages
                }, _uow.Transaction);
            }
            else
            {
                ids = await _db.QueryAsync<int>(selectProduct, new
                {
                    order.Type,
                    order.Name,
                    order.PersonId,
                    order.Pages
                });
            }
            var prodId = ids.FirstOrDefault();

            if (prodId == 0)
            {
                const string insertProd = @"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES(@PersonId,@Type,@Name,@Pages); SELECT CAST(SCOPE_IDENTITY() as int);";
                IEnumerable<int> prodIds;
                if (_uow != null)
                {
                    prodIds = await _uow.Connection.QueryAsync<int>(insertProd, new
                    {
                        order.PersonId,
                        order.Type,
                        order.Name,
                        order.Pages
                    }, _uow.Transaction);
                }
                else
                {
                    prodIds = await _db.QueryAsync<int>(insertProd, new
                    {
                        order.PersonId,
                        order.Type,
                        order.Name,
                        order.Pages
                    });
                }
                prodId = prodIds.First();
            }

            const string sql = @"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price)
                                   VALUES(@ProdId,@PersonId,@Printery,GETDATE(),@DateStart,@DateFinish,@Status,@Tirage,@Price)";

            int orderId;
            if (_uow != null)
            {
                orderId = await _uow.Connection.QuerySingleAsync<int>(sql + "; SELECT CAST(SCOPE_IDENTITY() as int);", new
                {
                    ProdId = prodId,
                    order.PersonId,
                    Printery = order.Printery,
                    order.DateStart,
                    order.DateFinish,
                    Status = order.Status.ToString(),
                    order.Tirage,
                    order.Price
                }, _uow.Transaction);
            }
            else
            {
                var idsSingle = await _db.QueryAsync<int>(sql + "; SELECT CAST(SCOPE_IDENTITY() as int);", new
                {
                    ProdId = prodId,
                    order.PersonId,
                    Printery = order.Printery,
                    order.DateStart,
                    order.DateFinish,
                    Status = order.Status.ToString(),
                    order.Tirage,
                    order.Price
                });
                orderId = idsSingle.First();
            }

            return orderId;
        }

        public Task UpdateExpiredAsync()
        {
            const string sql = "UPDATE Orders SET statusOrder = 'завершено' WHERE statusOrder <> 'завершено' AND dateFinish < GETDATE()";
            return _db.ExecuteAsync(sql);
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

        public Task DeleteAsync(int id)
        {
            return _db.ExecuteAsync("DELETE FROM Orders WHERE idOrder = @id", new { id });
        }

        public Task DeleteLatestAsync(string personId)
        {
            const string sql = @"DELETE FROM Orders WHERE idOrder = (
                SELECT TOP 1 idOrder FROM Orders WHERE idPerson = @id ORDER BY idOrder DESC)";
            return _db.ExecuteAsync(sql, new { id = personId });
        }

        public Task UpdateStatusAsync(int id, string status)
        {
            const string sql = "UPDATE Orders SET statusOrder = @status WHERE idOrder = @id";
            return _db.ExecuteAsync(sql, new { status, id });
        }

        public async Task<string?> GetPersonIdAsync(int id)
        {
            const string sql = "SELECT CAST(idPerson AS varchar) FROM Orders WHERE idOrder = @id";
            var ids = await _db.QueryAsync<string>(sql, new { id });
            return ids.FirstOrDefault();
        }
    }
}
