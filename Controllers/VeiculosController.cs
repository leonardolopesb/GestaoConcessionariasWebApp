using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Veiculos;
using GestaoConcessionariasWebApp.Models.Veiculos.Create;
using GestaoConcessionariasWebApp.Models.Veiculos.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class VeiculosController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public VeiculosController(ApplicationDbContext db) => _db = db;

    // GET: api/veiculos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _db.Veiculos
            .AsNoTracking()
            .ToListAsync();

        return Ok(lista);
    }

    // GET: api/veiculos/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var v = await _db.Veiculos
            .Include(x => x.Fabricante)
            .FirstOrDefaultAsync(x => x.Id == id);

        return v is null ? NotFound() : Ok(v);
    }

    // POST: api/veiculos
    [HttpPost]
    //[Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
    public async Task<IActionResult> Post([FromBody] CreateVeiculoDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var existeFabricante = await _db.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!existeFabricante) return BadRequest("Fabricante inválido.");

        var veiculo = Veiculo.Create(dto.Modelo, dto.AnoFabricacao, dto.Preco,
                                     dto.FabricanteId, dto.TipoVeiculo, dto.Descricao);

        _db.Veiculos.Add(veiculo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
    }

    // PUT: api/veiculos/{id}
    [HttpPut("{id:guid}")]
    //[Authorize(Roles = $"{Roles.Admin},{Roles.Gerente}")]
    /*
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateVeiculoDto dto)
    {
        var v = await _db.Veiculos.FindAsync(id);
        if (v is null) return NotFound();

        var existeFabricante = await _db.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);
        if (!existeFabricante) return BadRequest("Fabricante inválido.");

        v.Update(dto.Modelo, dto.AnoFabricacao, dto.Preco, dto.FabricanteId, dto.TipoVeiculo, dto.Descricao);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    */

    // DELETE (soft): api/veiculos/{id}
    [HttpDelete("{id:guid}")]
    //[Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var v = await _db.Veiculos.FindAsync(id);
        if (v is null) return NotFound();

        v.Delete();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/veiculos/deleted
    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Veiculos
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .OrderBy(v => v.Modelo)
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/veiculos/{id}/restore
    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var v = await _db.Veiculos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (v is null) return NotFound();
        if (!v.IsDeleted) return BadRequest("Item não está deletado.");

        v.Restore();
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
