namespace Publishing.Infrastructure.DataAccess;

public sealed class DeleteOrderCommand : SqlCommand
{
    public DeleteOrderCommand(int id) => Id = id;
    public int Id { get; }

    public override string Sql => "DELETE FROM Orders WHERE idOrder = @Id";
    public override object Parameters => new { Id };
}
