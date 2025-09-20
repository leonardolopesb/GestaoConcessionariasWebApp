using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Fabricantes.Create;
using GestaoConcessionariasWebApp.Models.Fabricantes.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FabricantesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public FabricantesController(ApplicationDbContext db) => _db = db;

    // GET: api/Fabricantes
    [HttpGet]
    [Authorize(Roles = "Admin, Gerente, Vendedor")]
    public async Task<IActionResult> GetAll()
    {
        var fabricantes = await _db.Fabricantes
            .AsNoTracking()
            .ToListAsync();

        return Ok(fabricantes);
    }

    // GET: api/Fabricantes/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin, Gerente, Vendedor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var fabricante = await _db.Fabricantes.FindAsync(id);

        return fabricante is null ? NotFound() : Ok(fabricante);
    }

    // POST: api/Fabricantes
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromBody] CreateFabricanteDto dto)
    {
        var duplicado = await _db.Fabricantes
            .IgnoreQueryFilters()
            .AnyAsync(f => f.NomeFabricante == dto.NomeFabricante && !f.IsDeleted);

        if (duplicado)
            return Conflict("Já existe um fabricante com esse nome.");

        var fabricante = Fabricante.Create(
            dto.NomeFabricante,
            dto.PaisOrigem, 
            dto.AnoFundacao,
            dto.Website
        );

        _db.Fabricantes.Add(fabricante);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, fabricante);
    }

    // PUT: api/Fabricantes/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateFabricanteDto dto)
    {
        var fabricante = await _db.Fabricantes.FindAsync(id);

        if (fabricante is null)
            return NotFound();

        fabricante.Update(
            dto.NomeFabricante,
            dto.PaisOrigem,
            dto.AnoFundacao,
            dto.Website
        );

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Fabricantes/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var fabricante = await _db.Fabricantes.FindAsync(id);

        if (fabricante == null)
            return NotFound();

        fabricante.Delete();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Fabricantes/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Fabricantes
            .IgnoreQueryFilters()
            .Where(f => f.IsDeleted)
            .OrderBy(f => f.NomeFabricante)
            .AsNoTracking()
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/Fabricantes/{id}/restore
    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var fabricante = await _db.Fabricantes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fabricante is null)
            return NotFound();

        if (!fabricante.IsDeleted)
            return BadRequest("Item não está deletado.");

        fabricante.Restore();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
