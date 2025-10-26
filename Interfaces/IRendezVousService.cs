using spaV1.Models;

namespace spaV1.Interfaces
{
    public interface IRendezVousService
    {
        Task<IEnumerable<RendezVous>> GetAllRendezVousAsync();
        Task<RendezVous> GetRendezVousByIdAsync(int id);
        Task<IEnumerable<RendezVous>> GetRendezVousByUserIdAsync(int userId);
        Task<RendezVous> CreateRendezVousAsync(RendezVous rendezVous);
        Task UpdateRendezVousAsync(RendezVous rendezVous);
        Task DeleteRendezVousAsync(int id);
    }
}