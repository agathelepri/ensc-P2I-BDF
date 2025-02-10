using BDF.Data; 
/* using BDF.DTO; */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDF.Controllers;

[ApiController]
[Route("api/voeu")]
public class VoeuController : ControllerBase
{
    private readonly DataContext _context;

    public VoeuController(DataContext context)
    {
        _context = context;
    }

    // GET: api/voeu
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VoeuDTO>>> GetVoeux()
    {
        // Get courses and related lists
        var voeux = _context.Voeux.Select(x => new VoeuDTO(x));
        return await voeux.ToListAsync();
    }

    // GET: api/voeu/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VoeuDTO>> GetVoeu(int id)
    {
        // Find course and related list
        // SingleAsync() throws an exception if no course is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var voeu = await _context.Voeux.SingleOrDefaultAsync(t => t.Id == id);

        if (voeu == null)
        {
            return NotFound();
        }

        return new VoeuDTO(voeu);
    }

    // POST: api/voeu
    [HttpPost]
    public async Task<ActionResult<VoeuDTO>> PostVoeu(VoeuDTO voeuDTO)
    {
        Voeu voeu = new(voeuDTO);

        _context.Voeux.Add(voeu);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVoeu), new { id = voeu.Id }, new VoeuDTO(voeu));
    }

    // PUT: api/voeu/id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVoeu(int id, VoeuDTO voeuDTO)
    {
        if (id != voeuDTO.Id)
        {
            return BadRequest();
        }

        Voeu voeu = new(voeuDTO);

        _context.Entry(voeu).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Voeux.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/voeu/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVoeu(int id)
    {
        var voeu = await _context.Voeux.FindAsync(id);

        if (voeu == null)
        {
            return NotFound();
        }

        _context.Voeux.Remove(voeu);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
