using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spaV1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace spaV1.Controllers
{
    [Authorize(Roles = "Gerant")]
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get top services first
            var topServices = await _context.RendezVous
                .Where(r => r.Service != null)
                .GroupBy(r => r.ServiceId)
                .Select(g => new TopServiceViewModel
                {
                    ServiceName = g.First().Service.NomService,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            var viewModel = new StatisticsViewModel
            {
                TotalServices = await _context.Services.CountAsync(),
                TotalEmployees = await _context.Users.CountAsync(u => u.Role == "Employee"),
                TotalClients = await _context.Users.CountAsync(u => u.Role == "Client"),
                TotalAppointments = await _context.RendezVous.CountAsync(),
                TotalRevenue = await _context.Paiements.SumAsync(p => p.Montant),
                TopServices = topServices
            };

            return View(viewModel);
        }
    }
}