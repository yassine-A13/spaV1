using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using spaV1.Models;
using spaV1.Interfaces;

namespace spaV1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string role)
        {
            var users = await _userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => 
                u.Email == email && 
                u.Password == password && 
                u.Role.ToLower() == role.ToLower());

            if (user != null)
            {
                // Dans un vrai système, vous utiliseriez une authentification appropriée avec hachage du mot de passe
                TempData["UserId"] = user.Id;
                TempData["UserRole"] = user.Role;
                TempData["UserName"] = $"{user.Prenom} {user.Nom}";

                switch (user.Role.ToLower())
                {
                    case "gerant":
                        return RedirectToAction("GerantDashboard");
                    case "employee":
                        return RedirectToAction("EmployeeDashboard");
                    case "client":
                        return RedirectToAction("ClientDashboard");
                    default:
                        return RedirectToAction("Index");
                }
            }

            TempData["Error"] = "Email ou rôle incorrect";
            return RedirectToAction("Index");
        }

        public IActionResult GerantDashboard()
        {
            if (TempData["UserRole"]?.ToString()?.ToLower() != "gerant")
            {
                return RedirectToAction("Index");
            }
            ViewBag.UserName = TempData["UserName"];
            return View();
        }

        public IActionResult EmployeeDashboard()
        {
            if (TempData["UserRole"]?.ToString()?.ToLower() != "employee")
            {
                return RedirectToAction("Index");
            }
            ViewBag.UserName = TempData["UserName"];
            return View();
        }

        public IActionResult ClientDashboard()
        {
            if (TempData["UserRole"]?.ToString()?.ToLower() != "client")
            {
                return RedirectToAction("Index");
            }
            ViewBag.UserName = TempData["UserName"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
