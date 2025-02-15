using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VirtualRealityEscapeRooms.Models;

namespace VirtualRealityEscapeRooms
{
    public class IgrasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IgrasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Igras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Igre.ToListAsync());
        }

        // GET: Igras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igra = await _context.Igre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (igra == null)
            {
                return NotFound();
            }

            return View(igra);
        }

        // GET: Igras/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Reserve(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var korisnik = _context.Korisnici.FirstOrDefault(k => k.UserId == user.Id);
            if (korisnik == null) return NotFound();

            var rezervacija = new Rezervacija
            {
                KorisnikID = korisnik.ID,
                IgraID = id,
                DatumRezervacije = DateTime.Now,
                Status = "Pending"
            };

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Rezervacija uspješno kreirana!";
            return RedirectToAction("Index", "Igras");
        }

        // POST: Igras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Naziv,Tezina,Trajanje,Opis,Zanr")] Igra igra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(igra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(igra);
        }

        // GET: Igras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igra = await _context.Igre.FindAsync(id);
            if (igra == null)
            {
                return NotFound();
            }
            return View(igra);
        }

        // POST: Igras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Igra igra, IFormFile SlikaFile)
        {
            if (id != igra.ID)
            {
                return NotFound();
            }

            var existingGame = await _context.Igre.FindAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }

            if (SlikaFile != null && SlikaFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(SlikaFile.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await SlikaFile.CopyToAsync(stream);
                }


                igra.SlikaPath = "/images/" + fileName;
            }
            else
            {

                igra.SlikaPath = existingGame.SlikaPath;
            }


            existingGame.Naziv = igra.Naziv;
            existingGame.Tezina = igra.Tezina;
            existingGame.Trajanje = igra.Trajanje;
            existingGame.Opis = igra.Opis;
            existingGame.Zanr = igra.Zanr;
            existingGame.SlikaPath = igra.SlikaPath;

            try
            {
                _context.Update(existingGame);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Igre.Any(e => e.ID == igra.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }


        // GET: Igras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igra = await _context.Igre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (igra == null)
            {
                return NotFound();
            }

            return View(igra);
        }

        // POST: Igras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igra = await _context.Igre.FindAsync(id);
            if (igra != null)
            {
                _context.Igre.Remove(igra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IgraExists(int id)
        {
            return _context.Igre.Any(e => e.ID == id);
        }
    }
}
