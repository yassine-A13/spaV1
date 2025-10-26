using Microsoft.EntityFrameworkCore;
using spaV1.Interfaces;
using spaV1.Models;

namespace spaV1.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly ApplicationDbContext _context;

        public PaiementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Paiement>> GetAllPaiementsAsync()
        {
            return await _context.Paiements
                .Include(p => p.RendezVous)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<Paiement> GetPaiementByIdAsync(int id)
        {
            return await _context.Paiements
                .Include(p => p.RendezVous)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Paiement>> GetPaiementsByRendezVousIdAsync(int rendezVousId)
        {
            return await _context.Paiements
                .Where(p => p.RendezVousId == rendezVousId)
                .ToListAsync();
        }

        public async Task<Paiement> CreatePaiementAsync(Paiement paiement)
        {
            _context.Paiements.Add(paiement);
            await _context.SaveChangesAsync();
            return paiement;
        }

        public async Task UpdatePaiementAsync(Paiement paiement)
        {
            _context.Entry(paiement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePaiementAsync(int id)
        {
            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement != null)
            {
                _context.Paiements.Remove(paiement);
                await _context.SaveChangesAsync();
            }
        }
    }
}