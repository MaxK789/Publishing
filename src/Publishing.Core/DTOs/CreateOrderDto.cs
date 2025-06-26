namespace Publishing.Core.DTOs
{
    public class CreateOrderDto
    {
        public string Type { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int Tirage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateFinish { get; set; }
        public decimal Price { get; set; }
        public string Printery { get; set; } = string.Empty;
        public string PersonId { get; set; } = string.Empty;
    }
}
