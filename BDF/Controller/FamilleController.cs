using BDF.Data; 
/* using BDF.DTO; */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDF.Controllers;

[ApiController]
[Route("api/famille")]
public class FamilleController : ControllerBase
{
    private readonly DataContext _context;

    public FamilleController(DataContext context)
    {
        _context = context;
    }

    // GET: api/famille
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FamilleDTO>>> GetFamilles()
    {
        // Get courses and related lists
        var familles = _context.Familles.Select(x => new FamilleDTO(x));
        return await familles.ToListAsync();
    }

    // GET: api/famille/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<FamilleDTO>> GetFamille(int id)
    {
        // Find course and related list
        // SingleAsync() throws an exception if no course is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var famille = await _context.Familles.SingleOrDefaultAsync(t => t.Id == id);

        if (famille == null)
        {
            return NotFound();
        }

        return new FamilleDTO(famille);
    }

    // POST: api/famille
    [HttpPost]
    public async Task<ActionResult<FamilleDTO>> PostFamille(FamilleDTO familleDTO)
    {
        Famille famille = new(familleDTO);

        _context.Familles.Add(famille);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFamille), new { id = famille.Id }, new FamilleDTO(famille));
    }
[HttpGet("classement")]
public async Task<IActionResult> GetClassement()
{
    var classement = await _context.Familles
        .OrderByDescending(c => c.Points) // Trie par points décroissants
        .Select(c => new
        {
            c.Id,
            c.Nom,
            c.Points,
            Couleur = c.CouleurHexa // Assure-toi que ce champ existe bien
        })
        .ToListAsync();

    if (classement.Count == 0)
    {
        return NotFound("Aucune donnée trouvée.");
    }
    await _context.SaveChangesAsync();

    return Ok(classement);
}


    // PUT: api/famille/id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutFamille(int id, FamilleDTO familleDTO)
    {
        if (id != familleDTO.Id)
        {
            return BadRequest();
        }

        Famille famille = new(familleDTO);

        _context.Entry(famille).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Familles.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/famille/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFamille(int id)
    {
        var famille = await _context.Familles.FindAsync(id);

        if (famille == null)
        {
            return NotFound();
        }

        _context.Familles.Remove(famille);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
