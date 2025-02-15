using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms.Models;
using VirtualRealityEscapeRooms.Data;
using Microsoft.AspNetCore.Authorization;

namespace VirtualRealityEscapeRooms.Controllers
{
    [Authorize(Roles = "User,Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Rezervacija
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rezervacija>>> GetRezervacije()
        {
            return await _context.Rezervacije.ToListAsync();
        }

        // GET: api/Rezervacija/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rezervacija>> GetRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return rezervacija;
        }

        // PUT: api/Rezervacija/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRezervacija(int id, Rezervacija rezervacija)
        {
            if (id != rezervacija.ID)
            {
                return BadRequest();
            }

            _context.Entry(rezervacija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervacijaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Rezervacija
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rezervacija>> PostRezervacija([FromBody] Rezervacija rezervacija)
        {
            if (rezervacija.KorisnikID <= 0 || rezervacija.IgraID <= 0)
                return BadRequest(new { error = "KorisnikID i IgraID moraju biti validni brojevi." });

            var korisnik = await _context.Korisnici.FindAsync(rezervacija.KorisnikID);
            var igra = await _context.Igre.FindAsync(rezervacija.IgraID);

            if (korisnik == null) return BadRequest(new { error = "Korisnik ne postoji." });
            if (igra == null) return BadRequest(new { error = "Igra ne postoji." });

            rezervacija.Korisnik = null; 
            rezervacija.Igra = null; 

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRezervacija", new { id = rezervacija.ID }, rezervacija);
        }

        // DELETE: api/Rezervacija/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRezervacija(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("zavrsi_igru")]
        public async Task<IActionResult> ZavrsiIgru([FromBody] RezultatIgre rezultat)
        {
            var korisnik = await _context.Korisnici.FindAsync(rezultat.KorisnikID);
            if (korisnik == null) return NotFound();

            korisnik.HighScore += rezultat.Bodovi;
            if (rezultat.Bodovi > korisnik.MaxScore)
            {
                korisnik.MaxScore = rezultat.Bodovi;
            }

            await _context.SaveChangesAsync();
            return Ok(new { poruka = "Rezultat ažuriran!" });
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.ID == id);
        }
    }
}
