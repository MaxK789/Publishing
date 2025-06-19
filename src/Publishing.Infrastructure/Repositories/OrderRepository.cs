using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbContext _db;

        public OrderRepository(IDbContext db)
        {
            _db = db;
        }

        public void Save(Publishing.Core.Domain.Order order)
        {
            const string selectProduct = @"SELECT idProduct FROM Product WHERE typeProduct = @Type AND nameProduct = @Name AND idPerson = @PersonId AND pagesNum = @Pages";
            var prodId = _db.QueryAsync<int>(selectProduct, new
            {
                order.Type,
                order.Name,
                order.PersonId,
                order.Pages
            }).Result.FirstOrDefault();

            if (prodId == 0)
            {
                const string insertProd = @"INSERT INTO Product(idPerson,typeProduct,nameProduct,pagesNum) VALUES(@PersonId,@Type,@Name,@Pages); SELECT CAST(SCOPE_IDENTITY() as int);";
                prodId = _db.QueryAsync<int>(insertProd, new
                {
                    order.PersonId,
                    order.Type,
                    order.Name,
                    order.Pages
                }).Result.First();
            }

            const string sql = @"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price)
                                   VALUES(@ProdId,@PersonId,@Printery,GETDATE(),@DateStart,@DateFinish,@Status,@Tirage,@Price)";

            _db.ExecuteAsync(sql, new
            {
                ProdId = prodId,
                order.PersonId,
                Printery = order.Printery,
                order.DateStart,
                order.DateFinish,
                Status = order.Status.ToString(),
                order.Tirage,
                order.Price
            }).Wait();
        }

        public Task UpdateExpiredAsync()
        {
            const string sql = "UPDATE Orders SET statusOrder = 'завершено' WHERE statusOrder <> 'завершено' AND dateFinish < GETDATE()";
            return _db.ExecuteAsync(sql);
        }

        public Task<DataTable> GetActiveAsync()
        {
            const string sql = @"SELECT O.namePrintery, Prod.typeProduct, Prod.nameProduct, Per.FName, Per.LName, O.dateOrder, O.dateStart, O.dateFinish, O.statusOrder, O.price FROM(Orders O INNER JOIN Product Prod ON Prod.idProduct = O.idProduct ) INNER JOIN Person Per ON Per.idPerson = Prod.idPerson WHERE O.statusOrder = 'в роботі' ORDER BY O.dateOrder";
            return DbContextExtensions.QueryDataTableAsync(_db, sql);
        }

        public Task<DataTable> GetByPersonAsync(string personId)
        {
            const string sql = "SELECT * FROM Orders where idPerson = @id";
            return DbContextExtensions.QueryDataTableAsync(_db, sql, new { id = personId });
        }

        public Task<DataTable> GetAllAsync()
        {
            return DbContextExtensions.QueryDataTableAsync(_db, "SELECT * FROM Orders");
        }

        public Task DeleteAsync(int id)
        {
            return _db.ExecuteAsync("DELETE FROM Orders WHERE idOrder = @id", new { id });
        }
    }
}
