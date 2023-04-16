using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
	public class AccountController : Controller
	{

		private readonly ApplicationDbContext _db;
		UserManager<ApplicationUser> _userManager;
		SignInManager<ApplicationUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;
        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
			_db = db;
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;

        }

        public IActionResult Login()
		{
			return View();
		}
        public async Task<IActionResult> Register()
		{
			if (!_roleManager.RoleExistsAsync(Helper.Admin).GetAwaiter().GetResult()) {
				await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
				await _roleManager.CreateAsync(new IdentityRole(Helper.Manager));
				await _roleManager.CreateAsync(new IdentityRole(Helper.Associate));
			}
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			// Server side validation
			if (ModelState.IsValid) {
				var user = new ApplicationUser
				{
					UserName = model.Email,
					Email = model.Email,
					Name = model.Name
				};

				var result = await _userManager.CreateAsync(user);
				if (result.Succeeded) {
					await _userManager.AddToRoleAsync(user, model.RoleName);
					await _signInManager.SignInAsync(user, isPersistent: false);
					return RedirectToAction("Index", "Home");
				}
			}
			return View();

		}
	}
}
