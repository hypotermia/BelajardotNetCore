using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Value>>> GetValue()
        {
            return await _context.Value.ToListAsync();
        }

        // GET: api/Values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Value>> GetValue(int id)
        {
            var value = await _context.Value.FindAsync(id);

            if (value == null)
            {
                return NotFound();
            }

            return value;
        }

        // PUT: api/Values/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<IActionResult> PutValue(int id, Value value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _context.Entry(value).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ValueExists(id))
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

        // POST: api/Values
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Value>> PostValue(Value value)
        {
            _context.Value.Add(value);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValue", new { id = value.Id }, value);
        }

        // DELETE: api/Values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Value>> DeleteValue(int id)
        {
            var value = await _context.Value.FindAsync(id);
            if (value == null)
            {
                return NotFound();
            }

            _context.Value.Remove(value);
            await _context.SaveChangesAsync();

            return value;
        }

        private bool ValueExists(int id)
        {
            return _context.Value.Any(e => e.Id == id);
        }
    }
}
