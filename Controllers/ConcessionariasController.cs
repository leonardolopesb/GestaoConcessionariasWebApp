using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Concessionarias;
using GestaoConcessionariasWebApp.Models.Concessionarias.Create;
using GestaoConcessionariasWebApp.Models.Concessionarias.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ConcessionariasController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ConcessionariasController(ApplicationDbContext db) => _db = db;

    // GET: api/Concessionarias
    [HttpGet]
    [Authorize(Roles = "Admin, Gerente, Vendedor")]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _db.Concessionarias
            .AsNoTracking()
            .ToListAsync();

        return Ok(lista);
    }

    // GET: api/Concessionarias/{id}
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin, Gerente, Vendedor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var concessionaria = await _db.Concessionarias.FindAsync(id);

        return concessionaria is null ? NotFound() : Ok(concessionaria);
    }

    // POST: api/Concessionarias
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromBody] CreateConcessionariaDto dto)
    {
        var duplicado = await _db.Concessionarias
            .IgnoreQueryFilters()
            .AnyAsync(c => c.Nome == dto.Nome && !c.IsDeleted);

        if (duplicado)
            return Conflict("Já existe uma concessionária com esse nome.");

        var concessionaria = Concessionaria.Create(
            dto.Nome,
            dto.CEP,
            dto.Cidade,
            dto.Estado,
            dto.Endereco,
            dto.Telefone,
            dto.Email,
            dto.CapacidadeMaximaVeiculos);

        _db.Concessionarias.Add(concessionaria);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = concessionaria.Id }, concessionaria);
    }

    // PUT: api/Concessionarias/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateConcessionariaDto dto)
    {
        var concessionaria = await _db.Concessionarias.FindAsync(id);

        if (concessionaria is null) 
            return NotFound();

        concessionaria.Update(
            dto.Nome, 
            dto.CEP,
            dto.Estado, 
            dto.Cidade,
            dto.Endereco,
            dto.Telefone,
            dto.Email,
            dto.CapacidadeMaximaVeiculos);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Concessionarias/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var concessionaria = await _db.Concessionarias.FindAsync(id);

        if (concessionaria is null)
            return NotFound();

        concessionaria.Delete();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Concessionarias/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Concessionarias
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .OrderBy(x => x.Nome)
            .AsNoTracking()
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/Concessionarias/{id}/restore
    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var concessionarias = await _db.Concessionarias
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (concessionarias is null) 
            return NotFound();

        if (!concessionarias.IsDeleted) 
            return BadRequest("Item não está deletado.");

        concessionarias.Restore();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
