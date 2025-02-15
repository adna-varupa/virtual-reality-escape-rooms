using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms.Data;

namespace VirtualRealityEscapeRooms
{
    public class PrijavaKorisnikasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijavaKorisnikasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrijavaKorisnikas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrijaveKorisnika.Include(p => p.Prijavio).Include(p => p.Prijavljeni);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrijavaKorisnikas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaKorisnika = await _context.PrijaveKorisnika
                .Include(p => p.Prijavio)
                .Include(p => p.Prijavljeni)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prijavaKorisnika == null)
            {
                return NotFound();
            }

            return View(prijavaKorisnika);
        }

        // GET: PrijavaKorisnikas/Create
        public IActionResult Create()
        {
            ViewData["PrijavioId"] = new SelectList(_context.Korisnici, "ID", "Email");
            ViewData["PrijavljeniId"] = new SelectList(_context.Korisnici, "ID", "Email");
            return View();
        }

        // POST: PrijavaKorisnikas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PrijavioId,PrijavljeniId,Razlog,Status,DatumPrijave")] PrijavaKorisnika prijavaKorisnika)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prijavaKorisnika);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PrijavioId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavioId);
            ViewData["PrijavljeniId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavljeniId);
            return View(prijavaKorisnika);
        }

        // GET: PrijavaKorisnikas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaKorisnika = await _context.PrijaveKorisnika.FindAsync(id);
            if (prijavaKorisnika == null)
            {
                return NotFound();
            }
            ViewData["PrijavioId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavioId);
            ViewData["PrijavljeniId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavljeniId);
            return View(prijavaKorisnika);
        }

        // POST: PrijavaKorisnikas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PrijavioId,PrijavljeniId,Razlog,Status,DatumPrijave")] PrijavaKorisnika prijavaKorisnika)
        {
            if (id != prijavaKorisnika.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prijavaKorisnika);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrijavaKorisnikaExists(prijavaKorisnika.ID))
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
            ViewData["PrijavioId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavioId);
            ViewData["PrijavljeniId"] = new SelectList(_context.Korisnici, "ID", "Email", prijavaKorisnika.PrijavljeniId);
            return View(prijavaKorisnika);
        }

        // GET: PrijavaKorisnikas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavaKorisnika = await _context.PrijaveKorisnika
                .Include(p => p.Prijavio)
                .Include(p => p.Prijavljeni)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prijavaKorisnika == null)
            {
                return NotFound();
            }

            return View(prijavaKorisnika);
        }

        // POST: PrijavaKorisnikas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prijavaKorisnika = await _context.PrijaveKorisnika.FindAsync(id);
            if (prijavaKorisnika != null)
            {
                _context.PrijaveKorisnika.Remove(prijavaKorisnika);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrijavaKorisnikaExists(int id)
        {
            return _context.PrijaveKorisnika.Any(e => e.ID == id);
        }
    }
}
