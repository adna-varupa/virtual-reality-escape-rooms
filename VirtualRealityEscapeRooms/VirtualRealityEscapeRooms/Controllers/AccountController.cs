using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtualRealityEscapeRooms.Data;
using VirtualRealityEscapeRooms.Models;
using VirtualRealityEscapeRooms.ViewModels;
using VR_escape_rooms.modeli;

namespace VirtualRealityEscapeRooms.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Ime = model.Ime,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }
     
            await _userManager.AddToRoleAsync(user, model.Role);

            var korisnik = new Korisnik
            {
                Ime = model.Ime,
                Email = model.Email,
                TipKorisnika = model.Role,
                HighScore = 0,
                MaxScore = 0,
                UserId = user.Id 
            };

            _context.Korisnici.Add(korisnik);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);

            return model.Role == "Admin"
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.Contains("Admin")
                ? RedirectToAction("Dashboard", "Admin")
                : RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
