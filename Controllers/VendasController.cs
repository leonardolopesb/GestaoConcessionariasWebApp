using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Clientes;
using GestaoConcessionariasWebApp.Models.Vendas;
using GestaoConcessionariasWebApp.Models.Vendas.Create;
using GestaoConcessionariasWebApp.Models.Vendas.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Gerente, Vendedor")]
public sealed class VendasController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    public VendasController(ApplicationDbContext db) => _db = db;

    #region GETs opcionais que auxiliam a API de Vendas

    // GET api/Vendas/fabricantes
    [HttpGet("fabricantes")]
    public async Task<IActionResult> SearchFabricantes([FromQuery] string? nome)
    {
        nome ??= string.Empty;
        var list = await _db.Fabricantes
            .AsNoTracking()
            .Where(f => f.NomeFabricante.Contains(nome))
            .OrderBy(f => f.NomeFabricante)
            .Select(f => new { f.Id, f.NomeFabricante })
            .Take(30)
            .ToListAsync();

        return Ok(list);
    }

    // GET api/Vendas/modelos
    [HttpGet("modelos")]
    public async Task<IActionResult> GetModelosByFabricante([FromQuery] string fabricanteNome)
    {
        if (string.IsNullOrWhiteSpace(fabricanteNome))
            return BadRequest("Fabricante obrigatório.");

        var fab = await _db.Fabricantes.AsNoTracking()
            .FirstOrDefaultAsync(f => f.NomeFabricante == fabricanteNome);
        if (fab is null) return NotFound("Fabricante não encontrado.");

        var modelos = await _db.Veiculos.AsNoTracking()
            .Where(v => v.FabricanteId == fab.Id)
            .OrderBy(v => v.Modelo)
            .Select(v => new { v.Id, v.Modelo, v.Preco })
            .ToListAsync();

        return Ok(modelos);
    }

    // GET api/Vendas/concessionarias
    [HttpGet("concessionarias")]
    public async Task<IActionResult> SearchConcessionarias([FromQuery] string? q)
    {
        q ??= string.Empty;
        var list = await _db.Concessionarias
            .AsNoTracking()
            .Where(c =>
                c.Nome.Contains(q) ||
                c.Cidade.Contains(q) ||
                c.Estado.Contains(q) ||
                c.Endereco.Contains(q))
            .OrderBy(c => c.Nome)
            .Select(c => new { c.Id, c.Nome, c.Cidade, c.Estado })
            .Take(30)
            .ToListAsync();

        return Ok(list);
    }

    #endregion

    // GET: api/Vendas
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dados = await _db.Vendas
            .AsNoTracking()
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

        return Ok(dados);
    }

    // GET: api/Vendas/{id}
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
        var cpf = Regex.Replace(dto.CpfCliente ?? "", @"\D", "");
        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.CPF == cpf);

        if (cliente is null)
            return BadRequest("Cliente não encontrado.");

        // Protocolo
        string NovoProtocolo20() =>
            $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}".Substring(0, 20).ToUpper();

        string protocolo;

        do { protocolo = NovoProtocolo20(); }
        while (await _db.Vendas.IgnoreQueryFilters().AnyAsync(x => x.ProtocoloVenda == protocolo));

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

    // DELETE: api/Vendas/{id}
    [HttpDelete("{id:guid}")]
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