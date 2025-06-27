using System.Data;
using Publishing.Core.Domain;

namespace Publishing.Infrastructure.DataAccess;

public sealed class GetActiveOrdersQuery : SqlQuery<Order>
{
    public override string Sql =>
        @"SELECT O.namePrintery, Prod.typeProduct, Prod.nameProduct, Per.FName, Per.LName, O.dateOrder, O.dateStart, O.dateFinish, O.statusOrder, O.price
          FROM (Orders O INNER JOIN Product Prod ON Prod.idProduct = O.idProduct)
          INNER JOIN Person Per ON Per.idPerson = Prod.idPerson WHERE O.statusOrder IN ('в роботі', 'InProgress') ORDER BY O.dateOrder";

    public override Order Map(IDataReader reader)
    {
        return new Order
        {
            Printery = reader["namePrintery"].ToString() ?? string.Empty,
            Type = reader["typeProduct"].ToString() ?? string.Empty,
            Name = reader["nameProduct"].ToString() ?? string.Empty,
            PersonId = string.Empty,
            DateOrder = (DateTime)reader["dateOrder"],
            DateStart = (DateTime)reader["dateStart"],
            DateFinish = (DateTime)reader["dateFinish"],
            Status = Enum.TryParse<OrderStatus>(reader["statusOrder"].ToString(), out var s) ? s : OrderStatus.InProgress,
            Price = (decimal)reader["price"]
        };
    }
}
