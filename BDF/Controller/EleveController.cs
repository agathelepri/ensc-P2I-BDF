using BDF.Data;
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

    // 🔹 Récupérer tous les élèves
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleveDTO>>> GetEleve()
    {
        var eleves = await _context.Eleves
            .Select(e => new EleveDTO(e))
            .ToListAsync();

        return Ok(eleves);
    }

    // 🔹 Récupérer un élève par son ID
    [HttpGet("{id}")]
    public async Task<ActionResult<EleveDTO>> GetEleve(int id)
    {
        var eleve = await _context.Eleves.SingleOrDefaultAsync(t => t.Id == id);

        if (eleve == null)
        {
            return NotFound();
        }

        return new EleveDTO(eleve);
    }

    //Vérifier si l'élève existe et s'il a un mot de passe (première connexion)
    [HttpPost("check-user")]
    public async Task<IActionResult> CheckUser([FromBody] EleveDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
            return NotFound(new { error = "Utilisateur non trouvé." });

        return Ok(new { firstLogin = string.IsNullOrEmpty(eleve.MDP), userId = eleve.Id }); //Retourne `userId`
    }


    // Enregistrer un mot de passe lors de la première connexion
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] EleveDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
            return NotFound(new { error = "Utilisateur non trouvé." });

        eleve.MDP = eleveDTO.MDP; 
        await _context.SaveChangesAsync();

        return Ok(new { message = "Mot de passe enregistré avec succès !", userId = eleve.Id }); // Retourne `userId`
    }

    // Connexion : Vérification du mot de passe
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] EleveDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
            return NotFound(new { error = "Utilisateur non trouvé." });

        if (eleve.MDP != eleveDTO.MDP) 
            return Unauthorized(new { error = "Mot de passe incorrect." });

        return Ok(new { success = true, userId = eleve.Id }); // Retourne `userId`
    }


    // Ajouter un nouvel élève
    [HttpPost]
    public async Task<ActionResult<EleveDTO>> PostEleve(EleveDTO eleveDTO)
    {
        if (eleveDTO == null)
            return BadRequest("Les données de l'élève sont invalides.");

        Eleve eleve = new Eleve(eleveDTO);
        _context.Eleves.Add(eleve);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEleve), new { id = eleve.Id }, new EleveDTO(eleve));
    }

    // Modifier un élève existant
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

    // Supprimer un élève
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
