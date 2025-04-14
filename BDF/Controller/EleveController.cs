// Ce contrôleur gère toutes les opérations liées aux élèves :
// - Création, modification, suppression d'élève
// - Connexion avec gestion de mot de passe (haché)
// - Affectation du parrain et de la famille
// - Récupération des filleuls pour un parrain donné
// Il permet aussi de récupérer les élèves par promotion et de vérifier l'état de connexion.

using BDF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BDF.Controllers;

[ApiController]
[Route("api/eleve")]
public class EleveController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration; // Pour les paramètres JWT

    public EleveController(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // Récupérer tous les élèves
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleveDTO>>> GetEleve()
    {
        var eleves = await _context.Eleves
            .Include(e => e.Promotion)
            .Include(e => e.Famille)
            .Include(e => e.EleveParrain)
            .Select(e => new EleveDTO(e))
            .ToListAsync();

        return Ok(eleves);
    }

    // Récupérer un élève par son ID
    [HttpGet("{id}")]
    public async Task<ActionResult<EleveDTO>> GetEleve(int id)
    {
        Console.WriteLine($" Recherche de l'élève avec ID : {id}");
        
        var eleve = await _context.Eleves
            .Include(e => e.Promotion)
            .Include(e => e.Famille)
            .Include(e => e.EleveParrain)
            .SingleOrDefaultAsync(t => t.Id == id);

        if (eleve == null)
        {
            Console.WriteLine(" Élève non trouvé.");
            return NotFound();
        }

        return new EleveDTO(eleve);
    }

    // Vérifier si l'élève existe et s'il a un mot de passe
    [HttpPost("check-user")]
    public async Task<IActionResult> CheckUser([FromBody] CheckUserDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
        {
            return NotFound(new { error = "Utilisateur non trouvé." });
        }
        await _context.SaveChangesAsync();

        return Ok(new { firstLogin = string.IsNullOrEmpty(eleve.MDP), userId = eleve.Id });
    }

    // Enregistrer un mot de passe haché lors de la première connexion
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] CheckUserDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
        {
            return NotFound(new { error = "Utilisateur non trouvé." });
        }

        // Hachage du mot de passe
        eleve.MDP = HashPassword(eleveDTO.MDP);
        _context.Entry(eleve).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Mot de passe enregistré avec succès !", userId = eleve.Id });
    }


    [HttpGet("promotion/{promotionId}")]
public async Task<IActionResult> GetElevesByPromotion(int promotionId)
{
    var eleves = await _context.Eleves
        .Where(e => e.PromotionId == promotionId)
        .Include(e => e.EleveParrain)
        .Include(e => e.Promotion)
        .Include(e => e.Famille)
        .Select(e => new EleveDTO(e))
        .ToListAsync();

    var allFilleuls = await _context.Eleves
        .Include(e => e.EleveParrain)
        .Where(e => e.EleveParrain != null)
        .Select(e => new EleveDTO(e))
        .ToListAsync();

             
    return Ok(eleves);
}

[HttpGet("filleuls/{parrainId}")]
public async Task<IActionResult> GetFilleuls(int parrainId)
{
    var filleuls = await _context.Eleves
        .Where(e => e.EleveParrain != null && e.EleveParrain.Id == parrainId)
        .Select(e => new
        {
            e.Id,
            e.Nom,
            e.Prenom
        })
        .ToListAsync();
        _context.Entry(filleuls).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return Ok(filleuls);
}

    [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] CheckUserDTO eleveDTO)
{
    var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

    if (eleve == null)
    {
        return NotFound(new { error = "Utilisateur non trouvé." });
    }

    if (eleve.MDP != HashPassword(eleveDTO.MDP))
    {
        return Unauthorized(new { error = "Mot de passe incorrect." });
    }

    // Vérifie si l'utilisateur est Admin 
    bool isAdmin = eleve.Login.ToLower() == "admin";
    _context.Entry(eleve).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return Ok(new { success = true, userId = eleve.Id, role = isAdmin ? "admin" : "user" });
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
   [HttpPut("{id}")]
public async Task<IActionResult> PutEleve(int id, EleveDTO eleveDTO)
{
    if (eleveDTO == null || id != eleveDTO.Id)
        return BadRequest("Identifiant invalide ou données manquantes.");

    var eleve = await _context.Eleves
        .Include(e => e.Promotion)
        .Include(e => e.Famille)
        .Include(e => e.EleveParrain)
        .FirstOrDefaultAsync(e => e.Id == id);

    if (eleve == null)
        return NotFound($"Aucun élève trouvé avec l'ID {id}.");

    // Mise à jour des champs simples
    eleve.Nom = eleveDTO.Nom;
    eleve.Prenom = eleveDTO.Prenom;
    eleve.Login = eleveDTO.Login;
    eleve.MDP = eleveDTO.MDP;
    eleve.Photo = eleveDTO.Photo;

    // Mise à jour des relations
    if (eleveDTO.PromotionId != 0)
    {
        var promotion = await _context.Promotions.FindAsync(eleveDTO.PromotionId);
        if (promotion == null) return BadRequest("Promotion non trouvée.");
        eleve.Promotion = promotion;
    }

    if (eleveDTO.FamilleId != 0)
    {
        var famille = await _context.Familles.FindAsync(eleveDTO.FamilleId);
        if (famille == null) return BadRequest("Famille non trouvée.");
        eleve.Famille = famille;
        eleve.FamilleId = famille.Id;
    }

    if (eleveDTO.EleveParrainId != 0)
    {
        var parrain = await _context.Eleves.FindAsync(eleveDTO.EleveParrainId);
    if (parrain == null)
        return BadRequest("Parrain non trouvé.");

    eleve.EleveParrainId = eleveDTO.EleveParrainId; 
}
else
{
    eleve.EleveParrainId = null; 
}
_context.Entry(eleve).State=EntityState.Modified;
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!_context.Eleves.Any(e => e.Id == id))
            return NotFound($"Impossible de mettre à jour : l'élève avec l'ID {id} n'existe plus.");
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

    // Hachage sécurisé du mot de passe
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    // Génération d'un token JWT
    private string GenerateJwtToken(Eleve eleve)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, eleve.Id.ToString()),
                new Claim(ClaimTypes.Name, eleve.Login)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
