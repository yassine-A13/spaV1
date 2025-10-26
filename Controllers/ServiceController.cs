using Microsoft.AspNetCore.Mvc;
using spaV1.Interfaces;
using spaV1.Models;

namespace spaV1.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceSpa _serviceSpa;

        public ServiceController(IServiceSpa serviceSpa)
        {
            _serviceSpa = serviceSpa;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _serviceSpa.GetAllServicesAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomService,TypeService,Prix")] Service service)
        {
            if (ModelState.IsValid)
            {
                await _serviceSpa.CreateServiceAsync(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var service = await _serviceSpa.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomService,TypeService,Prix")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _serviceSpa.UpdateServiceAsync(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _serviceSpa.DeleteServiceAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}