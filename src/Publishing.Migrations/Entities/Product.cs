namespace Publishing.Migrations.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }

        public Person Person { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
