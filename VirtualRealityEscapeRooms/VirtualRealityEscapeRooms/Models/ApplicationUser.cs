using Microsoft.AspNetCore.Identity;

namespace VirtualRealityEscapeRooms.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public int? HighScore { get; set; }
        public int? MaxScore { get; set; }
    }
}
