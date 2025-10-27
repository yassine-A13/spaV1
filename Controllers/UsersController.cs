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
		public async Task<IActionResult> Create(User user)
		{
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Le mot de passe doit contenir au moins 6 caractères");
                    return View(user);
                }

                try
                {
                    await _userService.CreateUserAsync(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création de l'utilisateur : " + ex.Message);
                }
            }
            return View(user);
		}

		// Détails d'un utilisateur
		public async Task<IActionResult> Details(int id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// Formulaire d'édition
		public async Task<IActionResult> Edit(int id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		// Enregistrement des modifications
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, User user)
		{
			if (id != user.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					await _userService.UpdateUserAsync(user);
					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la mise à jour : " + ex.Message);
				}
			}
			return View(user);
		}

		// Affiche la confirmation de suppression
		public async Task<IActionResult> Delete(int id)
		{
			var user = await _userService.GetUserByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			return View(user);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _userService.DeleteUserAsync(id);
			return RedirectToAction(nameof(Index));
		}
	}
}
