using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VirtualRealityEscapeRooms.Data;

namespace VirtualRealityEscapeRooms.Views.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalUsers { get; set; }
        public int TotalReservations { get; set; }
        public int TotalGames { get; set; }

        public void OnGet()
        {
            TotalUsers = _context.Korisnici.Count();
            TotalReservations = _context.Rezervacije.Count();
            TotalGames = _context.Igre.Count();
        }
    }
}
