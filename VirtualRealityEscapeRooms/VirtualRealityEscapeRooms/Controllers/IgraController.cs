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
    public class IgraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IgraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Igra
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Igra>>> GetIgre()
        {
            return await _context.Igre.ToListAsync();
        }

        // GET: api/Igra/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Igra>> GetIgra(int id)
        {
            var igra = await _context.Igre.FindAsync(id);

            if (igra == null)
            {
                return NotFound();
            }

            return igra;
        }

        // PUT: api/Igra/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIgra(int id, Igra igra)
        {
            if (id != igra.ID)
            {
                return BadRequest();
            }

            _context.Entry(igra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IgraExists(id))
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

        // POST: api/Igra
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Igra>> PostIgra(Igra igra)
        {
            _context.Igre.Add(igra);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIgra", new { id = igra.ID }, igra);
        }

        // DELETE: api/Igra/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIgra(int id)
        {
            var igra = await _context.Igre.FindAsync(id);
            if (igra == null)
            {
                return NotFound();
            }

            _context.Igre.Remove(igra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("filtriraj")]
        public async Task<ActionResult<IEnumerable<Igra>>> FiltrirajIgre(
            [FromQuery] string tezina = null,
            [FromQuery] string zanr = null)
        {
            var query = _context.Igre.AsQueryable();

            if (Enum.TryParse<Tezina>(tezina, out Tezina parsedTezina))
            {
                query = query.Where(i => i.Tezina == parsedTezina);
            }

            if (!string.IsNullOrEmpty(zanr))
            {
                if (Enum.TryParse<Zanr>(zanr, true, out var parsedZanr)) // True to ignore case
                {
                    query = query.Where(i => i.Zanr == parsedZanr);
                }
                else
                {
                    return BadRequest("Invalid zanr value.");
                }
            }


            return await query.ToListAsync();
        }

        private bool IgraExists(int id)
        {
            return _context.Igre.Any(e => e.ID == id);
        }
    }
}
