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
[Authorize]
public sealed class ClientesController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public ClientesController(ApplicationDbContext db) => _db = db;

    // GET: api/Clientes
    [HttpGet]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _db.Clientes
            .AsNoTracking()
            .ToListAsync();

        return Ok(lista);
    }

    // GET: api/Clientes/{id}
    [Authorize(Roles = "Gerente, Vendedor")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cliente = await _db.Clientes.FindAsync(id);

        return cliente is null ? NotFound() : Ok(cliente);
    }

    // GET: api/Clientes/by-cpf/{cpf}   
    [HttpGet("by-cpf/{cpf}")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var cliente = await _db.Clientes.FirstOrDefaultAsync(x => x.CPF == Regex.Replace(cpf ?? "", @"\D", ""));

        return cliente is null ? NotFound() : Ok(cliente);
    }

    // POST: api/Clientes
    [HttpPost]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> Post([FromBody] CreateClienteDto dto)
    {
        var cpf = Regex.Replace(dto.CPF ?? "", @"\D", "");
        var cliente = Cliente.Create(dto.Nome, cpf, dto.Telefone);

        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    // PUT: api/Clientes/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateClienteDto dto)
    {
        var cliente = await _db.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound();

        cliente.Update(
            dto.Nome,
            dto.CPF,
            dto.Telefone);

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Clientes/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var cliente = await _db.Clientes.FindAsync(id);

        if (cliente is null)
            return NotFound();

        cliente.Delete();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Clientes/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Clientes
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .OrderBy(x => x.Nome)
            .AsNoTracking()
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/Clientes/{id}/restore
    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var cliente = await _db.Clientes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (cliente is null)
            return NotFound();

        if (!cliente.IsDeleted)
            return BadRequest("O cliente não está deletado.");

        cliente.Restore();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
