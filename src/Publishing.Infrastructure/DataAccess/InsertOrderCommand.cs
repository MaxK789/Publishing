namespace Publishing.Infrastructure.DataAccess;

public sealed class InsertOrderCommand : SqlCommand
{
    public InsertOrderCommand(int productId, string personId, string printery, DateTime start, DateTime finish, string status, int tirage, decimal price)
    {
        ProductId = productId;
        PersonId = personId;
        Printery = printery;
        DateStart = start;
        DateFinish = finish;
        Status = status;
        Tirage = tirage;
        Price = price;
    }

    public int ProductId { get; }
    public string PersonId { get; }
    public string Printery { get; }
    public DateTime DateStart { get; }
    public DateTime DateFinish { get; }
    public string Status { get; }
    public int Tirage { get; }
    public decimal Price { get; }

    public override string Sql =>
        @"INSERT INTO Orders(idProduct,idPerson,namePrintery,dateOrder,dateStart,dateFinish,statusOrder,tirage,price)
          VALUES(@ProductId,@PersonId,@Printery,GETDATE(),@DateStart,@DateFinish,@Status,@Tirage,@Price)";

    public override object Parameters => new
    {
        ProductId,
        PersonId,
        Printery,
        DateStart,
        DateFinish,
        Status,
        Tirage,
        Price
    };
}
