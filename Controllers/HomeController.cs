using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using spaV1.Models;
using spaV1.Interfaces;
using Microsoft.AspNetCore.Http;

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
            // Hide the navbar on the login page
            ViewData["HideNavbar"] = true;
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
                // Store user information in session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", $"{user.Prenom} {user.Nom}");

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

        // Restrict access to dashboards based on role
        public IActionResult GerantDashboard()
        {
            if (HttpContext.Session.GetString("UserRole")?.ToLower() != "gerant")
            {
                return Unauthorized();
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult EmployeeDashboard()
        {
            if (HttpContext.Session.GetString("UserRole")?.ToLower() != "employee")
            {
                return Unauthorized();
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult ClientDashboard()
        {
            if (HttpContext.Session.GetString("UserRole")?.ToLower() != "client")
            {
                return Unauthorized();
            }
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
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
