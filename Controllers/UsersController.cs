using Microsoft.AspNetCore.Mvc;
using spaV1.Models;
using spaV1.Interfaces;

namespace spaV1.Controllers
{
	public class UsersController : Controller
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		// Affiche la liste des utilisateurs
		public async Task<IActionResult> Index()
		{
			var users = await _userService.GetAllUsersAsync();
			return View(users);
		}

		// Formulaire d'ajout
		public IActionResult Create()
		{
			return View();
		}

		// Ajout en base
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Nom,Prenom,Email,Telephone,Role")] User user)
		{
			if (ModelState.IsValid)
			{
				await _userService.CreateUserAsync(user);
				return RedirectToAction(nameof(Index));
			}
			return View(user);
		}
	}
}
