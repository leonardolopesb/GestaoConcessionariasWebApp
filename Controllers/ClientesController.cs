using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Clientes;
using GestaoConcessionariasWebApp.Models.Clientes.Create;
using GestaoConcessionariasWebApp.Models.Clientes.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ClientesController(ApplicationDbContext db) => _db = db;

    // GET: api/clientes
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? q)
    {
        var query = _db.Clientes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var qcpf = Regex.Replace(q, @"\D", "");
            query = query.Where(c => c.Nome.Contains(q) || c.CPF.Contains(qcpf));
        }

        var lista = await query.OrderBy(c => c.Nome).Take(100).ToListAsync();
        return Ok(lista);
    }

    // GET: api/clientes/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var c = await _db.Clientes.FindAsync(id);
        return c is null ? NotFound() : Ok(c);
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateClienteDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var cpf = Regex.Replace(dto.CPF ?? "", @"\D", "");
        var existe = await _db.Clientes
            .IgnoreQueryFilters()
            .AnyAsync(x => x.CPF == cpf && !x.IsDeleted);

        if (existe) return Conflict("Já existe cliente com esse CPF.");

        try
        {
            var c = Cliente.Create(dto.Nome, cpf, dto.Telefone);
            _db.Clientes.Add(c);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, c);
        }
        catch (ArgumentException ex)
        {
            return ValidationProblem(title: "Dados inválidos", detail: ex.Message);
        }
    }

    // PUT: api/clientes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateClienteDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();

        var cpf = Regex.Replace(dto.CPF ?? "", @"\D", "");
        var cpfEmUso = await _db.Clientes
            .IgnoreQueryFilters()
            .AnyAsync(x => x.Id != id && !x.IsDeleted && x.CPF == cpf);

        if (cpfEmUso) return Conflict("CPF já cadastrado para outro cliente.");

        try
        {
            c.Update(dto.Nome, cpf, dto.Telefone);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return ValidationProblem(title: "Dados inválidos", detail: ex.Message);
        }
    }

    // DELETE (soft): api/clientes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var c = await _db.Clientes.FindAsync(id);
        if (c is null) return NotFound();

        c.Delete();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/clientes/deleted
    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Clientes
            .IgnoreQueryFilters().Where(x => x.IsDeleted)
            .OrderBy(x => x.Nome)
            .AsNoTracking().ToListAsync();

        return Ok(itens);
    }

    // POST: api/clientes/{id}/restore
    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var c = await _db.Clientes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();
        if (!c.IsDeleted) return BadRequest("Item não está deletado.");

        c.Restore();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/clientes/by-cpf/12345678901
    [HttpGet("by-cpf/{cpf}")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var cpfNum = Regex.Replace(cpf ?? "", @"\D", "");
        var c = await _db.Clientes.FirstOrDefaultAsync(x => x.CPF == cpfNum);
        return c is null ? NotFound() : Ok(c);
    }
}
