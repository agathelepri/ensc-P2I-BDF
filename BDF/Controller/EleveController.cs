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

/*     // Connexion : Vérification sécurisée du mot de passe et génération d'un token JWT
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CheckUserDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
        {
            return NotFound(new { error = "Utilisateur non trouvé." });
        }

        // Vérification du mot de passe haché
        if (eleve.MDP != HashPassword(eleveDTO.MDP))
        {
            return Unauthorized(new { error = "Mot de passe incorrect." });
        }

        // Générer un token JWT
        var token = GenerateJwtToken(eleve);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, userId = eleve.Id, token });
        
    } */
    [HttpGet("promotion/{promotionId}")]
public async Task<IActionResult> GetElevesByPromotion(int promotionId)
{
    var eleves = await _context.Eleves
        .Where(e => e.PromotionId == promotionId)
        .Include(e => e.EleveParrain)
        .ToListAsync();

    var allFilleuls = await _context.Eleves
        .Include(e => e.EleveParrain)
        .Where(e => e.EleveParrain != null)
        .ToListAsync();

    var result = eleves.Select(e =>
    {
        var filleuls = allFilleuls
            .Where(f => f.EleveParrain?.Id == e.Id)
            .Select(f => new { f.Nom, f.Prenom })
            .ToList();

        return new
        {
            e.Id,
            e.Nom,
            e.Prenom,
            e.Login,
            Affichage = promotionId == 1
                ? (object)filleuls // liste de filleuls
                : (e.EleveParrain != null ? new { e.EleveParrain.Nom, e.EleveParrain.Prenom } : null)
        };
    });

    return Ok(result);
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

    // Vérifie si l'utilisateur est Admin (Ex: le compte "admin")
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
    if (id != eleveDTO.Id)
    {
        return BadRequest();
    }

    var eleve = await _context.Eleves
        .Include(e => e.EleveParrain)
        .FirstOrDefaultAsync(e => e.Id == id);

    if (eleve == null)
    {
        return NotFound();
    }

    // Mise à jour des champs simples
    eleve.Nom = eleveDTO.Nom;
    eleve.Prenom = eleveDTO.Prenom;
    eleve.Login = eleveDTO.Login;

    // Mise à jour du parrain
    if (eleveDTO.EleveParrainId != 0)
    {
        var parrain = await _context.Eleves.FindAsync(eleveDTO.EleveParrainId);
        eleve.EleveParrain = parrain;
    }
    else
    {
        eleve.EleveParrain = null;
    }

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
