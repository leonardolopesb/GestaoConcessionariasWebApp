using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Fabricantes.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FabricantesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public FabricantesController(ApplicationDbContext db) => _db = db;


        // GET: api/fabricantes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fabricantes = await _db.Fabricantes.AsNoTracking().ToListAsync();

            return Ok(fabricantes);
        }

        // GET: api/fabricantes/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var fabricante = await _db.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();

            return Ok(fabricante);
        }

        // GET: api/fabricantes/deleted
        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeleted()
        {
            var itens = await _db.Fabricantes
                .IgnoreQueryFilters()
                .Where(f => f.IsDeleted)
                .AsNoTracking()
                .OrderBy(f => f.Nome)
                .ToListAsync();

            return Ok(itens);
        }

        // POST: api/fabricantes
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateFabricanteDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var fabricante = Fabricante.Create(
                dto.Nome,
                dto.PaisOrigem, 
                dto.AnoFundacao,
                dto.Website
            );

            _db.Fabricantes.Add(fabricante);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, fabricante);
        }

        // POST: api/fabricantes/{id}/restore
        [HttpPost("{id:guid}/restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            var fabricante = await _db.Fabricantes
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fabricante is null) return NotFound();
            if (!fabricante.IsDeleted) return BadRequest("Item não está deletado.");

            typeof(Fabricante)
                .GetProperty(nameof(fabricante.IsDeleted))!
                .SetValue(fabricante, false);

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/fabricantes/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var fabricante = await _db.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();

            fabricante.Delete();
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
