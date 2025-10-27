using spaV1.Models;

namespace spaV1.Models
{
    public class RendezVous
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public Paiement? Paiement { get; set; } // relation 1-1
        // Status of the rendez-vous: Pending, Accepted, Refused
        public string? Status { get; set; } = "Pending";
    }
}
