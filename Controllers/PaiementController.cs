using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using spaV1.Interfaces;
using spaV1.Models;

namespace spaV1.Controllers
{
    public class PaiementController : Controller
    {
        private readonly IPaiementService _paiementService;
        private readonly IRendezVousService _rendezVousService;

        public PaiementController(IPaiementService paiementService, IRendezVousService rendezVousService)
        {
            _paiementService = paiementService;
            _rendezVousService = rendezVousService;
        }

        public async Task<IActionResult> Index()
        {
            var paiements = await _paiementService.GetAllPaiementsAsync();
            return View(paiements);
        }

        public async Task<IActionResult> Create()
        {
            var rendezVous = await _rendezVousService.GetAllRendezVousAsync();
            ViewBag.RendezVous = new SelectList(rendezVous, "Id", "Id", null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatePaiement,Montant,RendezVousId")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _paiementService.CreatePaiementAsync(paiement);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erreur lors de la création du paiement : " + ex.Message);
                }
            }

            // Collect ModelState errors for display
            ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            var rendezVous = await _rendezVousService.GetAllRendezVousAsync();
            ViewBag.RendezVous = new SelectList(rendezVous, "Id", "Id", paiement.RendezVousId);
            return View(paiement);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var paiement = await _paiementService.GetPaiementByIdAsync(id);
            if (paiement == null)
            {
                return NotFound();
            }

            var rendezVous = await _rendezVousService.GetAllRendezVousAsync();
            ViewBag.RendezVous = new SelectList(rendezVous, "Id", "Id", paiement.RendezVousId);
            return View(paiement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatePaiement,Montant,RendezVousId")] Paiement paiement)
        {
            if (id != paiement.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _paiementService.UpdatePaiementAsync(paiement);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Erreur lors de la modification du paiement : " + ex.Message);
                }
            }

            ViewBag.ModelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

            var rendezVous = await _rendezVousService.GetAllRendezVousAsync();
            ViewBag.RendezVous = new SelectList(rendezVous, "Id", "Id", paiement.RendezVousId);
            return View(paiement);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _paiementService.DeletePaiementAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}