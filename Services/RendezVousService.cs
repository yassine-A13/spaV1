using Microsoft.EntityFrameworkCore;
using spaV1.Interfaces;
using spaV1.Models;

namespace spaV1.Services
{
    public class RendezVousService : IRendezVousService
    {
        private readonly ApplicationDbContext _context;

        public RendezVousService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RendezVous>> GetAllRendezVousAsync()
        {
            return await _context.RendezVous
                .Include(r => r.User)
                .Include(r => r.Service)
                .ToListAsync();
        }

        public async Task<RendezVous> GetRendezVousByIdAsync(int id)
        {
            return await _context.RendezVous
                .Include(r => r.User)
                .Include(r => r.Service)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RendezVous>> GetRendezVousByUserIdAsync(int userId)
        {
            return await _context.RendezVous
                .Include(r => r.Service)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<RendezVous> CreateRendezVousAsync(RendezVous rendezVous)
        {
            _context.RendezVous.Add(rendezVous);
            await _context.SaveChangesAsync();
            return rendezVous;
        }

        public async Task UpdateRendezVousAsync(RendezVous rendezVous)
        {
            _context.Entry(rendezVous).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRendezVousAsync(int id)
        {
            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous != null)
            {
                _context.RendezVous.Remove(rendezVous);
                await _context.SaveChangesAsync();
            }
        }
    }
}