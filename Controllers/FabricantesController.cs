using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models;
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


        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Fabricantes.AsNoTracking().OrderBy(f => f.Nome).ToListAsync());


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Fabricante request)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var fabricante = new Fabricante
            {
                Nome = request.Nome,
                PaisOrigem = request.PaisOrigem,
                AnoFundacao = request.AnoFundacao,
                Website = request.Website
            };

            _db.Fabricantes.Add(fabricante);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, fabricante);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var f = await _db.Fabricantes.FindAsync(id);
            return f is null ? NotFound() : Ok(f);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var f = await _db.Fabricantes.FindAsync(id);
            if (f is null) return NotFound();
            f.IsDeleted = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
