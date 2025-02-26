using BDF.Data; 
/* using BDF.DTO; */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDF.Controllers;

[ApiController]
[Route("api/questionnaire")]
public class QuestionnaireController : ControllerBase
{
    private readonly DataContext _context;

    public QuestionnaireController(DataContext context)
    {
        _context = context;
    }

    // GET: api/questionnaire
    /* [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionnaireDTO>>> GetQuestionnaires()
    {
        // Get courses and related lists
        var questionnaires = _context.Questionnaires.Select(x => new QuestionnaireDTO(x));
        return await questionnaires.ToListAsync();
    } */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionnaireDTO>>> GetQuestionnaires()
    {
        var questionnaires = await _context.Questionnaires
            .Include(q => q.Eleve) // Assure que Eleve est bien chargé
            .Select(x => new QuestionnaireDTO(x))
            .ToListAsync();

        return Ok(questionnaires);
    }


    // GET: api/questionnaire/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionnaireDTO>> GetQuestionnaire(int id)
    {
        // Find course and related list
        // SingleAsync() throws an exception if no course is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var questionnaire = await _context.Questionnaires.SingleOrDefaultAsync(t => t.Id == id);

        if (questionnaire == null)
        {
            return NotFound();
        }

        return new QuestionnaireDTO(questionnaire);
    }

    [HttpGet("eleve/{eleveId}")]
    public async Task<ActionResult<IEnumerable<QuestionnaireDTO>>> GetQuestionnaireByEleve(int eleveId)
    {
        var questionnaires = await _context.Questionnaires
            .Where(q => q.EleveId == eleveId)
            .Select(q => new QuestionnaireDTO(q))
            .ToListAsync();

        if (questionnaires == null || questionnaires.Count == 0)
        {
            return NotFound("Aucun questionnaire trouvé pour cet élève.");
        }

        return Ok(questionnaires);
    }


    // POST: api/questionnaire
    /* [HttpPost]
    public async Task<ActionResult<QuestionnaireDTO>> PostQuestionnaire(QuestionnaireDTO questionnaireDTO)
    {
        Questionnaire questionnaire = new(questionnaireDTO);

        _context.Questionnaires.Add(questionnaire);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQuestionnaire), new { id = questionnaire.Id }, new QuestionnaireDTO(questionnaire));
    } */
    [HttpPost]
public async Task<IActionResult> PostQuestionnaire([FromBody] QuestionnaireDTO questionnaireDTO)
{
    if (questionnaireDTO == null)
    {
        return BadRequest("Les données du questionnaire sont invalides.");
    }

    try
    {
        // Vérifier si l'élève existe
        var eleve = await _context.Eleves.FindAsync(questionnaireDTO.Eleve);
        if (eleve == null)
        {
            return BadRequest("L'élève spécifié n'existe pas.");
        }

        // Créer le questionnaire et l'associer à l'élève
        var questionnaire = new Questionnaire
        {
            EleveId = eleve.Id, // Lier l'élève connecté au questionnaire
            Provenance = questionnaireDTO.Provenance,
            Astro = questionnaireDTO.Astro,
            Boisson = questionnaireDTO.Boisson,
            Soiree = questionnaireDTO.Soiree,
            Son = questionnaireDTO.Son,
            Livre = questionnaireDTO.Livre,
            Film = questionnaireDTO.Film,
            PasseTemps = questionnaireDTO.PasseTemps,
            Defaut = questionnaireDTO.Defaut,
            Qualite = questionnaireDTO.Qualite,
            Relation = questionnaireDTO.Relation,
            Preference = questionnaireDTO.Preference
        };

        _context.Questionnaires.Add(questionnaire);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Questionnaire enregistré avec succès !" });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Erreur serveur : {ex.InnerException?.Message ?? ex.Message}");
    }
}




    // PUT: api/questionnaire/id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutQuestionnaire(int id, QuestionnaireDTO questionnaireDTO)
    {
        if (id != questionnaireDTO.Id)
        {
            return BadRequest();
        }

        Questionnaire questionnaire = new(questionnaireDTO);

        _context.Entry(questionnaire).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Questionnaires.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/questionnaire/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestionnaire(int id)
    {
        var questionnaire = await _context.Questionnaires.FindAsync(id);

        if (questionnaire == null)
        {
            return NotFound();
        }

        _context.Questionnaires.Remove(questionnaire);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
