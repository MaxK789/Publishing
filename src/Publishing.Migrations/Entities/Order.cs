namespace Publishing.Migrations.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int PersonId { get; set; }
        public string NamePrintery { get; set; } = string.Empty;
        public DateTime DateOrder { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Tirage { get; set; }
        public int Price { get; set; }

        public Person Person { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
