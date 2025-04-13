// Ce contrôleur gère les vœux de parrainage faits par les élèves :
// - Chaque voeu associe un élève à un autre avec un rang de priorité (NumVoeux)
// - Il permet de créer, consulter, modifier et supprimer des vœux
// Ces informations sont essentielles pour l’algorithme de matching.

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
        var voeux = _context.Voeux.Include(x=>x.Eleve)
        .Include(x=>x.Promotion)
        .Include(x=>x.EleveChoisi).Select(x => new VoeuDTO(x));
        return await voeux.ToListAsync();
    }

    // GET: api/voeu/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<VoeuDTO>> GetVoeu(int id)
    {
        var voeu = await _context.Voeux.SingleOrDefaultAsync(t => t.Id == id);

        if (voeu == null)
        {
            return NotFound();
        }

        return new VoeuDTO(voeu);
    }

[HttpPost]
public async Task<IActionResult> PostVoeu(VoeuDTO dto)
{
    Console.WriteLine(dto.EleveId+" " +dto.EleveChoisiId+ " " +dto.PromotionId);
    var eleve = await _context.Eleves.FindAsync(dto.EleveId);
    var eleveChoisi = await _context.Eleves.FindAsync(dto.EleveChoisiId);
    var promotion = await _context.Promotions.FindAsync(dto.PromotionId);

    if (eleve == null || eleveChoisi == null || promotion == null)
    {
        return BadRequest("Élève ou promotion non trouvée");
    }

    var voeu = new Voeu
    {
        EleveId = eleve.Id,
        EleveChoisiId = eleveChoisi.Id,
        PromotionId = promotion.Id,
        NumVoeux = dto.NumVoeux
    };

    _context.Voeux.Add(voeu);
    await _context.SaveChangesAsync();

    return Ok();
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
