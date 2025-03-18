/* using BDF.Data;
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
            .Include(e => e.Promotion)
            .Include(e => e.Famille)
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
    public async Task<IActionResult> CheckUser([FromBody] CheckUserDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
            return NotFound(new { error = "Utilisateur non trouvé." });

        return Ok(new { firstLogin = string.IsNullOrEmpty(eleve.MDP), userId = eleve.Id }); //Retourne `userId`
    }


    // Enregistrer un mot de passe lors de la première connexion
    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword([FromBody] CheckUserDTO eleveDTO)
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
    public async Task<IActionResult> Login([FromBody] CheckUserDTO eleveDTO)
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
} */
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

    // 🔹 Récupérer tous les élèves
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleveDTO>>> GetEleve()
    {
        var eleves = await _context.Eleves
            .Include(e => e.Promotion)
            .Include(e => e.Famille)
            .Select(e => new EleveDTO(e))
            .ToListAsync();

        return Ok(eleves);
    }

    // 🔹 Récupérer un élève par son ID
    [HttpGet("{id}")]
    public async Task<ActionResult<EleveDTO>> GetEleve(int id)
    {
        Console.WriteLine($" Recherche de l'élève avec ID : {id}");
        
        var eleve = await _context.Eleves.SingleOrDefaultAsync(t => t.Id == id);

        if (eleve == null)
        {
            Console.WriteLine(" Élève non trouvé.");
            return NotFound();
        }

        return new EleveDTO(eleve);
    }

    // 🔹 Vérifier si l'élève existe et s'il a un mot de passe
    [HttpPost("check-user")]
    public async Task<IActionResult> CheckUser([FromBody] CheckUserDTO eleveDTO)
    {
        var eleve = await _context.Eleves.FirstOrDefaultAsync(e => e.Login == eleveDTO.Login);

        if (eleve == null)
        {
            return NotFound(new { error = "Utilisateur non trouvé." });
        }

        return Ok(new { firstLogin = string.IsNullOrEmpty(eleve.MDP), userId = eleve.Id });
    }

    // 🔹 Enregistrer un mot de passe haché lors de la première connexion
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

    // 🔹 Connexion : Vérification sécurisée du mot de passe et génération d'un token JWT
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
        
    }
    

    // 🔹 Ajouter un nouvel élève
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

    // 🔹 Modifier un élève existant
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEleve(int id, EleveDTO eleveDTO)
    {
        if (id != eleveDTO.Id)
        {
            return BadRequest();
        }

        Eleve eleve = new Eleve(eleveDTO);
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

    // 🔹 Supprimer un élève
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

    // 🔹 Hachage sécurisé du mot de passe
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    // 🔹 Génération d'un token JWT
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
