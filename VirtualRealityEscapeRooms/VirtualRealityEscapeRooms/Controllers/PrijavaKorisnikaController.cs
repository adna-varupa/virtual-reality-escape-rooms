using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms.Data;

namespace VirtualRealityEscapeRooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrijavaKorisnikaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrijavaKorisnikaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PrijavaKorisnika
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrijavaKorisnika>>> GetPrijaveKorisnika()
        {
            return await _context.PrijaveKorisnika.ToListAsync();
        }

        // GET: api/PrijavaKorisnika/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrijavaKorisnika>> GetPrijavaKorisnika(int id)
        {
            var prijavaKorisnika = await _context.PrijaveKorisnika.FindAsync(id);

            if (prijavaKorisnika == null)
            {
                return NotFound();
            }

            return prijavaKorisnika;
        }

        // PUT: api/PrijavaKorisnika/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrijavaKorisnika(int id, PrijavaKorisnika prijavaKorisnika)
        {
            if (id != prijavaKorisnika.ID)
            {
                return BadRequest();
            }

            _context.Entry(prijavaKorisnika).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrijavaKorisnikaExists(id))
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

        // POST: api/PrijavaKorisnika
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrijavaKorisnika>> PostPrijavaKorisnika([FromBody] PrijavaKorisnika prijavaKorisnika)
        {
            if (prijavaKorisnika.PrijavioId <= 0 || prijavaKorisnika.PrijavljeniId <= 0)
                return BadRequest(new { error = "PrijavioId i PrijavljeniId moraju biti validni brojevi." });

            var prijavio = await _context.Korisnici.FindAsync(prijavaKorisnika.PrijavioId);
            var prijavljeni = await _context.Korisnici.FindAsync(prijavaKorisnika.PrijavljeniId);

            if (prijavio == null) return BadRequest(new { error = "Korisnik koji je prijavio ne postoji." });
            if (prijavljeni == null) return BadRequest(new { error = "Korisnik koji je prijavljen ne postoji." });
            prijavaKorisnika.Prijavio = null;
            prijavaKorisnika.Prijavljeni = null;

            _context.PrijaveKorisnika.Add(prijavaKorisnika);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrijavaKorisnika", new { id = prijavaKorisnika.ID }, prijavaKorisnika);
        }

        // DELETE: api/PrijavaKorisnika/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrijavaKorisnika(int id)
        {
            var prijavaKorisnika = await _context.PrijaveKorisnika.FindAsync(id);
            if (prijavaKorisnika == null)
            {
                return NotFound();
            }

            _context.PrijaveKorisnika.Remove(prijavaKorisnika);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrijavaKorisnikaExists(int id)
        {
            return _context.PrijaveKorisnika.Any(e => e.ID == id);
        }
    }
}
