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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuestionnaireDTO>>> GetQuestionnaires()
    {
        // Get courses and related lists
        var questionnaires = _context.Questionnaires.Select(x => new QuestionnaireDTO(x));
        return await questionnaires.ToListAsync();
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

    // POST: api/questionnaire
    [HttpPost]
    public async Task<ActionResult<QuestionnaireDTO>> PostQuestionnaire(QuestionnaireDTO questionnaireDTO)
    {
        Questionnaire questionnaire = new(questionnaireDTO);

        _context.Questionnaires.Add(questionnaire);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetQuestionnaire), new { id = questionnaire.Id }, new QuestionnaireDTO(questionnaire));
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
