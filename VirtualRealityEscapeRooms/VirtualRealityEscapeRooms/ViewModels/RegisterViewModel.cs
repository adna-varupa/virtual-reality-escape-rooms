using System.ComponentModel.DataAnnotations;

namespace VirtualRealityEscapeRooms.ViewModels
{
    public class RegisterViewModel
    {
        public string Ime { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
