using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Vendas;
using GestaoConcessionariasWebApp.Models.Vendas.Create;
using GestaoConcessionariasWebApp.Models.Vendas.List;
using GestaoConcessionariasWebApp.Models.Vendas.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class VendasController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public VendasController(ApplicationDbContext db) => _db = db;

    // GET: api/Vendas
    [HttpGet]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetAll([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var query = _db.Vendas.AsNoTracking();

        if (start.HasValue) query = query.Where(v => v.DataVenda >= start.Value);
        if (end.HasValue) query = query.Where(v => v.DataVenda < end.Value);

        var venda = await query
            .Include(v => v.Veiculo)
            .Include(v => v.Concessionaria)
            .Include(v => v.Cliente)
            .OrderByDescending(v => v.DataVenda)
            .Select(v => new VendaListItemDto
            {
                Id = v.Id,
                VeiculoId = v.VeiculoId,
                ConcessionariaId = v.ConcessionariaId,
                ClienteId = v.ClienteId,
                DataVenda = v.DataVenda,
                PrecoVenda = v.PrecoVenda,
                Protocolo = v.ProtocoloVenda
            })
            .ToListAsync();

        return Ok(venda);
    }

    // GET: api/Vendas/{id}
    [Authorize(Roles = "Gerente, Vendedor")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var venda = await _db.Vendas
            .Include(x => x.Veiculo)
            .Include(x => x.Concessionaria)
            .Include(x => x.Cliente)
            .FirstOrDefaultAsync(x => x.Id == id);

        return venda is null ? NotFound() : Ok(venda);
    }

    // POST: api/Vendas
    [Authorize(Roles = "Vendedor")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateVendaDto dto)
    {
        // Concessionária (nome ou localização)
        var concessionaria = await _db.Concessionarias
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Nome == dto.ConcessionariaNomeOuLocalizacao ||
                x.Cidade == dto.ConcessionariaNomeOuLocalizacao ||
                x.Estado == dto.ConcessionariaNomeOuLocalizacao ||
                x.Endereco == dto.ConcessionariaNomeOuLocalizacao);

        if (concessionaria is null)
            return BadRequest("Concessionária não encontrada.");

        // Fabricante
        var fabricante = await _db.Fabricantes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.NomeFabricante == dto.FabricanteNome);

        if (fabricante is null)
            return BadRequest("Fabricante não encontrado.");

        // Veículo
        var veiculo = await _db.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.FabricanteId == fabricante.Id && x.Modelo == dto.VeiculoModelo);

        if (veiculo is null) 
            return BadRequest("Veículo não encontrado para esse fabricante e modelo.");

        // Cliente
        var cliente = await _db.Clientes
            .FirstOrDefaultAsync(c => c.CPF == dto.CpfCliente);

        if (cliente is null)
            return BadRequest("Cliente não encontrado.");

        // Protocolo
        string NovoProtocolo() =>
            $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 20).ToUpper();

        string protocolo;

        do { 
            protocolo = NovoProtocolo(); 
        } while (
            await _db.Vendas
                .IgnoreQueryFilters()
                .AnyAsync(x => x.ProtocoloVenda == protocolo)
                );

        // Venda
        var venda = Venda.Create(
            veiculo.Id,
            concessionaria.Id,
            cliente.Id,
            dto.DataVenda.ToUniversalTime(),
            dto.PrecoVenda,
            protocolo);

        _db.Vendas.Add(venda);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = venda.Id }, new
        {
            venda.Id,
            venda.VeiculoId,
            venda.ConcessionariaId,
            venda.ClienteId,
            venda.DataVenda,
            venda.PrecoVenda,
            venda.ProtocoloVenda
        });
    }

    // PUT: api/Vendas/{id}
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Vendedor")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateVendaDto dto)
    {
        var venda = await _db.Vendas.FindAsync(id);

        if (venda is null)
            return NotFound();

        venda.Update(
            dto.VeiculoId,
            dto.ConcessionariaId,
            dto.ClienteId,
            dto.DataVenda.ToUniversalTime(),
            dto.PrecoVenda
        );

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Vendas/{id}
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var venda = await _db.Vendas.FindAsync(id);

        if (venda is null)
            return NotFound();

        venda.Delete();
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/Vendas/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _db.Vendas
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .AsNoTracking()
            .Include(v => v.Veiculo)
            .Include(v => v.Concessionaria)
            .Include(v => v.Cliente)
            .OrderByDescending(v => v.DataVenda)
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/Vendas/{id}/restore
    [HttpPost("{id:guid}/restore")]
    [Authorize(Roles = "Gerente, Vendedor")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var venda = await _db.Vendas
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (venda is null)
            return NotFound();

        if (!venda.IsDeleted)
            return BadRequest("Item não está deletado.");

        venda.Restore();
        await _db.SaveChangesAsync();

        return NoContent();
    }
}