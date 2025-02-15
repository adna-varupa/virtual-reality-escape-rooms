using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VirtualRealityEscapeRooms.Data;
using VirtualRealityEscapeRooms.Models;
using VirtualRealityEscapeRooms.ViewModels;
using VR_escape_rooms.modeli;

namespace VirtualRealityEscapeRooms.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Ime = Input.Ime,
                    HighScore = 0,
                    MaxScore = 0
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.Role);

                    // 🔹 Ensure user is saved in AspNetUsers before using it
                    var savedUser = await _userManager.FindByEmailAsync(Input.Email);
                    if (savedUser == null)
                    {
                        Console.WriteLine("❌ User was not found after creation! Something went wrong.");
                        ModelState.AddModelError("", "Failed to save user in database.");
                        return Page();
                    }

                    // 🔹 Save user in Korisnik table
                    var korisnik = new Korisnik
                    {
                        Ime = Input.Ime,
                        Email = Input.Email,
                        TipKorisnika = Input.Role,
                        HighScore = 0,
                        MaxScore = 0,
                        UserId = savedUser.Id // Ensure ID is available
                    };

                    _context.Korisnici.Add(korisnik);
                    await _context.SaveChangesAsync();  // ✅ Ensure it is saved

                    Console.WriteLine("✅ User successfully created and added to Korisnik table.");

                    // Redirect based on role
                    return Input.Role == "Admin"
                        ? RedirectToAction("Dashboard", "Admin")
                        : RedirectToAction("Index", "Home");
                }

                // 🔹 Log detailed errors
                Console.WriteLine("❌ User registration failed:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error: {error.Code} - {error.Description}");
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                // 🔹 Log ModelState errors
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"❌ ModelState Error in {key}: {error.ErrorMessage}");
                    }
                }
            }

            return Page();
        }
    }
}
