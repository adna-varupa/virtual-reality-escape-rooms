using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VR_escape_rooms.modeli;
using VirtualRealityEscapeRooms.Data;
using VirtualRealityEscapeRooms.Models;

namespace VirtualRealityEscapeRooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcjenaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OcjenaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Ocjena
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ocjena>>> GetOcjene()
        {
            return await _context.Ocjene.ToListAsync();
        }

        // GET: api/Ocjena/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ocjena>> GetOcjena(int id)
        {
            var ocjena = await _context.Ocjene.FindAsync(id);

            if (ocjena == null)
            {
                return NotFound();
            }

            return ocjena;
        }

        // PUT: api/Ocjena/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOcjena(int id, Ocjena ocjena)
        {
            if (id != ocjena.ID)
            {
                return BadRequest();
            }

            _context.Entry(ocjena).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OcjenaExists(id))
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

        // POST: api/Ocjena
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ocjena>> PostOcjena([FromBody] Ocjena ocjena)
        {
            if (ocjena.KorisnikId <= 0 || ocjena.IgraId <= 0)
                return BadRequest(new { error = "KorisnikId i IgraId moraju biti validni brojevi." });

            var korisnik = await _context.Korisnici.FindAsync(ocjena.KorisnikId);
            var igra = await _context.Igre.FindAsync(ocjena.IgraId);

            if (korisnik == null) return BadRequest(new { error = "Korisnik ne postoji." });
            if (igra == null) return BadRequest(new { error = "Igra ne postoji." });

            ocjena.Korisnik = null; 
            ocjena.Igra = null; 

            _context.Ocjene.Add(ocjena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOcjena", new { id = ocjena.ID }, ocjena);
        }

        // DELETE: api/Ocjena/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOcjena(int id)
        {
            var ocjena = await _context.Ocjene.FindAsync(id);
            if (ocjena == null)
            {
                return NotFound();
            }

            _context.Ocjene.Remove(ocjena);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OcjenaExists(int id)
        {
            return _context.Ocjene.Any(e => e.ID == id);
        }
    }
}
