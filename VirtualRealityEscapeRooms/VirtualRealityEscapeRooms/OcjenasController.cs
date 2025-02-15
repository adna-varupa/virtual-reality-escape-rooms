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
    public class OcjenasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OcjenasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ocjenas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ocjene.Include(o => o.Igra).Include(o => o.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ocjenas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjene
                .Include(o => o.Igra)
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // GET: Ocjenas/Create
        public IActionResult Create()
        {
            ViewData["IgraId"] = new SelectList(_context.Igre, "ID", "ID");
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "ID", "Email");
            return View();
        }

        // POST: Ocjenas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,KorisnikId,IgraId,Ocijena,Komentar")] Ocjena ocjena)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ocjena);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IgraId"] = new SelectList(_context.Igre, "ID", "ID", ocjena.IgraId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "ID", "Email", ocjena.KorisnikId);
            return View(ocjena);
        }

        // GET: Ocjenas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjene.FindAsync(id);
            if (ocjena == null)
            {
                return NotFound();
            }
            ViewData["IgraId"] = new SelectList(_context.Igre, "ID", "ID", ocjena.IgraId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "ID", "Email", ocjena.KorisnikId);
            return View(ocjena);
        }

        // POST: Ocjenas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,KorisnikId,IgraId,Ocijena,Komentar")] Ocjena ocjena)
        {
            if (id != ocjena.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ocjena);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OcjenaExists(ocjena.ID))
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
            ViewData["IgraId"] = new SelectList(_context.Igre, "ID", "ID", ocjena.IgraId);
            ViewData["KorisnikId"] = new SelectList(_context.Korisnici, "ID", "Email", ocjena.KorisnikId);
            return View(ocjena);
        }

        // GET: Ocjenas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ocjena = await _context.Ocjene
                .Include(o => o.Igra)
                .Include(o => o.Korisnik)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (ocjena == null)
            {
                return NotFound();
            }

            return View(ocjena);
        }

        // POST: Ocjenas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ocjena = await _context.Ocjene.FindAsync(id);
            if (ocjena != null)
            {
                _context.Ocjene.Remove(ocjena);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OcjenaExists(int id)
        {
            return _context.Ocjene.Any(e => e.ID == id);
        }
    }
}
