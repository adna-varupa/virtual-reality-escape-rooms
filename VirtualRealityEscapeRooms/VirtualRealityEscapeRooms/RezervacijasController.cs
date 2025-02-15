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
    public class RezervacijasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RezervacijasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rezervacijas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rezervacije.Include(r => r.Igra).Include(r => r.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rezervacijas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Igra)
                .Include(r => r.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
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

        // POST: Rezervacijas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,KorisnikID,IgraID,DatumRezervacije,Status")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Rezervacija uspješno kreirana!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IgraID"] = new SelectList(_context.Igre, "ID", "ID", rezervacija.IgraID);
            ViewData["KorisnikID"] = new SelectList(_context.Korisnici, "ID", "Email", rezervacija.KorisnikID);
            return View(rezervacija);
        }

        // GET: Rezervacijas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["IgraID"] = new SelectList(_context.Igre, "ID", "ID", rezervacija.IgraID);
            ViewData["KorisnikID"] = new SelectList(_context.Korisnici, "ID", "Email", rezervacija.KorisnikID);
            return View(rezervacija);
        }

        // POST: Rezervacijas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,KorisnikID,IgraID,DatumRezervacije,Status")] Rezervacija rezervacija)
        {
            if (id != rezervacija.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.ID))
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
            ViewData["IgraID"] = new SelectList(_context.Igre, "ID", "ID", rezervacija.IgraID);
            ViewData["KorisnikID"] = new SelectList(_context.Korisnici, "ID", "Email", rezervacija.KorisnikID);
            return View(rezervacija);
        }

        // GET: Rezervacijas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Igra)
                .Include(r => r.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.ID == id);
        }
    }
}
