using Microsoft.AspNetCore.Mvc;
using spaV1.Models;

namespace spaV1.Controllers
{
	public class UsersController : Controller
	{
		private readonly ApplicationDbContext _context;

		public UsersController(ApplicationDbContext context)
		{
			_context = context;
		}

		// Affiche la liste des utilisateurs
		public IActionResult Index()
		{
			var users = _context.Users.ToList();
			return View(users);
		}

		// Formulaire d'ajout
		public IActionResult Create()
		{
			return View();
		}

		// Ajout en base
		[HttpPost]
		public IActionResult Create(User user)
		{
			if (ModelState.IsValid)
			{
				_context.Users.Add(user);
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}
	}
}
