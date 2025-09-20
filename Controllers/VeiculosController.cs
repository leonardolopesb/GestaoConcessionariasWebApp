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
[Authorize]
public class VeiculosController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public VeiculosController(ApplicationDbContext db) => _db = db;

    // GET: api/Veiculos
    [HttpGet]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _db.Veiculos
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .Select(v => new {
                v.Id,
                v.Modelo,
                v.AnoFabricacao,
                v.Preco,
                v.TipoVeiculo,
                v.Descricao,
                v.IsDeleted,
                v.FabricanteId,
                fabricanteNome = v.Fabricante.NomeFabricante
            })
            .ToListAsync();

        return Ok(lista);
    }

    // GET: api/Veiculos/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var veiculo = await _db.Veiculos
            .Include(x => x.Fabricante)
            .FirstOrDefaultAsync(x => x.Id == id);

        return veiculo is null ? NotFound() : Ok(veiculo);
    }

    // POST: api/Veiculos
    [HttpPost]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Post([FromBody] CreateVeiculoDto dto)
    {
        var existeFabricante = await _db.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);

        if (!existeFabricante)
            return BadRequest("Fabricante inválido.");

        var veiculo = Veiculo.Create(
            dto.Modelo,
            dto.AnoFabricacao,
            dto.Preco,
            dto.FabricanteId,
            dto.TipoVeiculo,
            dto.Descricao);

        _db.Veiculos.Add(veiculo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
    }

    // PUT: api/Veiculos/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Gerente")]
    
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateVeiculoDto dto)
    {
        var veiculo = await _db.Veiculos.FindAsync(id);

        if (veiculo is null)
            return NotFound();

        var existeFabricante = await _db.Fabricantes.AnyAsync(f => f.Id == dto.FabricanteId);

        if (!existeFabricante)
            return BadRequest("Fabricante inválido.");

        veiculo.Update(
            dto.Modelo, 
            dto.AnoFabricacao,
            dto.Preco, 
            dto.FabricanteId,
            dto.TipoVeiculo,
            dto.Descricao);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Veiculos/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var veiculo = await _db.Veiculos.FindAsync(id);

        if (veiculo is null)
            return NotFound();

        veiculo.Delete();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Veiculos/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Gerente")]
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

    // POST: api/Veiculos/{id}/restore
    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var veiculo = await _db.Veiculos
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (veiculo is null)
            return NotFound();

        if (!veiculo.IsDeleted)
            return BadRequest("Item não está deletado.");

        veiculo.Restore();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
