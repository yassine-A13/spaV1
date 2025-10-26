using spaV1.Models;

namespace spaV1.Interfaces
{
    public interface IPaiementService
    {
        Task<IEnumerable<Paiement>> GetAllPaiementsAsync();
        Task<Paiement> GetPaiementByIdAsync(int id);
        Task<IEnumerable<Paiement>> GetPaiementsByRendezVousIdAsync(int rendezVousId);
        Task<Paiement> CreatePaiementAsync(Paiement paiement);
        Task UpdatePaiementAsync(Paiement paiement);
        Task DeletePaiementAsync(int id);
    }
}