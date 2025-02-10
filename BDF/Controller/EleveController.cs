using BDF.Data;
/* using BDF.DTO; */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDF.Controllers;

[ApiController]
[Route("api/eleve")]
public class EleveController : ControllerBase
{
    private readonly DataContext _context;

    public EleveController(DataContext context)
    {
        _context = context;
    }

    // GET: api/eleve
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleveDTO>>> GetEleves()
    {
        // Get courses and related lists
        var eleves = _context.Eleves.Select(x => new EleveDTO(x));
        return await eleves.ToListAsync();
    }

    // GET: api/eleve/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<EleveDTO>> GetEleve(int id)
    {
        // Find course and related list
        // SingleAsync() throws an exception if no course is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var eleve = await _context.Eleves.SingleOrDefaultAsync(t => t.Id == id);

        if (eleve == null)
        {
            return NotFound();
        }

        return new EleveDTO(eleve);
    }

    // POST: api/eleve
    [HttpPost]
    public async Task<ActionResult<EleveDTO>> PostEleve(EleveDTO eleveDTO)
    {
        Eleve eleve = new(eleveDTO);

        _context.Eleves.Add(eleve);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEleve), new { id = eleve.Id }, new EleveDTO(eleve));
    }

    // PUT: api/eleve/id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEleve(int id, EleveDTO eleveDTO)
    {
        if (id != eleveDTO.Id)
        {
            return BadRequest();
        }

        Eleve eleve = new(eleveDTO);

        _context.Entry(eleve).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Eleves.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/eleve/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEleve(int id)
    {
        var eleve = await _context.Eleves.FindAsync(id);

        if (eleve == null)
        {
            return NotFound();
        }

        _context.Eleves.Remove(eleve);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}