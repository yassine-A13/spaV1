namespace spaV1.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string NomService { get; set; } = null!;
        public string TypeService { get; set; } = null!; // massage, soins, manucure...
        public decimal Prix { get; set; }
    }
}
