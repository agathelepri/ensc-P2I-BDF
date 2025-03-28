using BDF.Data; 
/* using BDF.DTO; */
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BDF.Controllers;

[ApiController]
[Route("api/promotion")]
public class PromotionController : ControllerBase
{
    private readonly DataContext _context;

    public PromotionController(DataContext context)
    {
        _context = context;
    }

    // GET: api/promotion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PromotionDTO>>> GetPromotions()
    {
        // Get courses and related lists
        var promotions = _context.Promotions.Select(x => new PromotionDTO(x));
        return await promotions.ToListAsync();
    }

    // GET: api/promotion/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PromotionDTO>> GetPromotion(int id)
    {
        // Find course and related list
        // SingleAsync() throws an exception if no course is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var promotion = await _context.Promotions.SingleOrDefaultAsync(t => t.Id == id);

        if (promotion == null)
        {
            return NotFound();
        }

        return new PromotionDTO(promotion);
    }
    [HttpGet("promotions")]
public async Task<IActionResult> GetPromotion()
{
    var promotion = await _context.Promotions
        .Select(p => new { p.Id, p.Annee })
        .ToListAsync();

    if (promotion.Count == 0)
    {
        return NotFound("Aucune promotion trouvée.");
    }
    await _context.SaveChangesAsync();

    return Ok(promotion);
}

    // POST: api/promotion
    [HttpPost]
    public async Task<ActionResult<PromotionDTO>> PostPromotion(PromotionDTO promotionDTO)
    {
        Promotion promotion = new(promotionDTO);

        _context.Promotions.Add(promotion);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPromotion), new { id = promotion.Id }, new PromotionDTO(promotion));
    }

    // PUT: api/promotion/id
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPromotion(int id, PromotionDTO promotionDTO)
    {
        if (id != promotionDTO.Id)
        {
            return BadRequest();
        }

        Promotion promotion = new(promotionDTO);

        _context.Entry(promotion).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Promotions.Any(m => m.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/promotion/id
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePromotion(int id)
    {
        var promotion = await _context.Promotions.FindAsync(id);

        if (promotion == null)
        {
            return NotFound();
        }

        _context.Promotions.Remove(promotion);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
