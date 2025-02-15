using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using VirtualRealityEscapeRooms.Data;
using Microsoft.AspNetCore.Authorization;
using VirtualRealityEscapeRooms.ViewModels;

namespace VirtualRealityEscapeRooms.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Statistika")]
    public class StatistikaController : Controller 
    {
        private readonly ApplicationDbContext _context;

        public StatistikaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var statistikaViewModel = new StatistikaViewModel
            {
                TotalUsers = await _context.Korisnici.CountAsync(),
                TotalGames = await _context.Igre.CountAsync(),
                TotalReservations = await _context.Rezervacije.CountAsync(),
                TotalReports = await _context.PrijaveKorisnika.CountAsync()
            };

            return View(statistikaViewModel);
        }
    }
}
