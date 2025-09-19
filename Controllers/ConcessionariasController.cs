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

    [HttpGet]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _db.Concessionarias
            .AsNoTracking()
            .ToListAsync();

        return Ok(lista);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Vendedor")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var c = await _db.Concessionarias
            .FindAsync(id);

        return c is null ? NotFound() : Ok(c);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromBody] CreateConcessionariaDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var nomeEmUso = await _db.Concessionarias
            .IgnoreQueryFilters()
            .AnyAsync(x => x.Nome == dto.Nome && !x.IsDeleted);

        if (nomeEmUso) return BadRequest("Já existe uma concessionária com esse nome.");

        var c = Concessionaria.Create(
            dto.Nome,
            dto.CEP,
            dto.Estado, 
            dto.Cidade,    
            dto.Endereco,
            dto.Telefone,
            dto.Email,
            dto.CapacidadeMaximaVeiculos);

        _db.Concessionarias.Add(c);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateConcessionariaDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var c = await _db.Concessionarias.FindAsync(id);
        if (c is null) return NotFound();

        var nomeEmUso = await _db.Concessionarias
            .IgnoreQueryFilters()
            .AnyAsync(x => x.Id != id && !x.IsDeleted && x.Nome == dto.Nome);

        if (nomeEmUso) return BadRequest("Nome já está em uso.");

        c.Update(
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

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var c = await _db.Concessionarias.FindAsync(id);
        if (c is null) return NotFound();

        c.Delete();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("deleted")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Concessionarias
            .IgnoreQueryFilters().Where(x => x.IsDeleted)
            .OrderBy(x => x.Nome).AsNoTracking().ToListAsync();
        return Ok(itens);
    }

    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var c = await _db.Concessionarias
            .IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);

        if (c is null) return NotFound();
        if (!c.IsDeleted) return BadRequest("Item não está deletado.");

        c.Restore();
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
