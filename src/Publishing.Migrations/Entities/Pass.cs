namespace Publishing.Migrations.Entities
{
    public class Pass
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
    }
}
