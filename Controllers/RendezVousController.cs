using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using spaV1.Interfaces;
using spaV1.Models;

namespace spaV1.Controllers
{
    public class RendezVousController : Controller
    {
        private readonly IRendezVousService _rendezVousService;
        private readonly IUserService _userService;
        private readonly IServiceSpa _serviceSpa;

        public RendezVousController(
            IRendezVousService rendezVousService,
            IUserService userService,
            IServiceSpa serviceSpa)
        {
            _rendezVousService = rendezVousService;
            _userService = userService;
            _serviceSpa = serviceSpa;
        }

        public async Task<IActionResult> Index()
        {
            var rendezVous = await _rendezVousService.GetAllRendezVousAsync();
            return View(rendezVous);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByIdAsync(id);
            if (rendezVous == null)
            {
                return NotFound();
            }
            return View(rendezVous);
        }

        public async Task<IActionResult> Create()
        {
            await PrepareViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RendezVous rendezVous)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure new rendez-vous starts as Pending
                    rendezVous.Status = rendezVous.Status ?? "Pending";
                    await _rendezVousService.CreateRendezVousAsync(rendezVous);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // capture DB or other exceptions to show in the view for debugging
                    ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de l'enregistrement : " + ex.Message);
                }
            }

            // If we get here, either ModelState was invalid or an exception occurred.
            // Collect ModelState errors to help debugging in the view.
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            ViewBag.ModelErrors = errors;

            await PrepareViewBags();
            return View(rendezVous);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByIdAsync(id);
            if (rendezVous == null)
            {
                return NotFound();
            }
            await PrepareViewBags();
            return View(rendezVous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RendezVous rendezVous)
        {
            if (id != rendezVous.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _rendezVousService.UpdateRendezVousAsync(rendezVous);
                return RedirectToAction(nameof(Index));
            }
            await PrepareViewBags();
            return View(rendezVous);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(int id)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByIdAsync(id);
            if (rendezVous == null) return NotFound();
            rendezVous.Status = "Accepted";
            await _rendezVousService.UpdateRendezVousAsync(rendezVous);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Refuse(int id)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByIdAsync(id);
            if (rendezVous == null) return NotFound();
            rendezVous.Status = "Refused";
            await _rendezVousService.UpdateRendezVousAsync(rendezVous);
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareViewBags()
        {
            var users = await _userService.GetAllUsersAsync();
            var services = await _serviceSpa.GetAllServicesAsync();
            
            ViewBag.Users = new SelectList(users, "Id", "Nom");
            ViewBag.Services = new SelectList(services, "Id", "NomService");
        }
    }
}