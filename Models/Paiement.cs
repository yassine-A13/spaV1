namespace spaV1.Models
{
    public class Paiement
    {
        public int Id { get; set; }
        public DateTime DatePaiement { get; set; }
        public decimal Montant { get; set; }

        public int RendezVousId { get; set; }
        public RendezVous? RendezVous { get; set; }
    }
}
