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

    // GET: api/vendas
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
                Protocolo = v.ProtocoloVenda,
                DataVenda = v.DataVenda,
                PrecoVenda = v.PrecoVenda,
                VeiculoModelo = v.Veiculo.Modelo,
                ConcessionariaNome = v.Concessionaria.Nome,
                ClienteNome = v.Cliente.Nome,
                ClienteCpf = v.Cliente.CPF
            })
            .ToListAsync();

        return Ok(dados);
    }

    // GET: api/vendas/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var v = await _db.Vendas
            .Include(x => x.Veiculo)
            .Include(x => x.Concessionaria)
            .Include(x => x.Cliente)
            .FirstOrDefaultAsync(x => x.Id == id);

        return v is null ? NotFound() : Ok(v);
    }

    // POST: api/vendas
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateVendaDto dto)
    {
        // Concessionária por nome
        var concessionaria = await _db.Concessionarias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Nome == dto.ConcessionariaNomeOuLocalizacao);

        if (concessionaria is null) return BadRequest("Concessionária não encontrada.");

        // Fabricante por nome
        var fabricante = await _db.Fabricantes
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.NomeFabricante == dto.FabricanteNomeOuModeloVeiculo);

        if (fabricante is null) return BadRequest("Fabricante não encontrado.");

        // Veículo por (FabricanteId, Modelo) -> retorna 1 por causa do índice único
        var veiculo = await _db.Veiculos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.FabricanteId == fabricante.Id && v.Modelo == dto.FabricanteNomeOuModeloVeiculo);

        if (veiculo is null) return BadRequest("Veículo não encontrado para esse fabricante e modelo.");

        // Preço da venda
        if (dto.PrecoVenda <= 0 || dto.PrecoVenda > veiculo.Preco)
            return BadRequest("Preço da venda deve ser positivo e menor que o preço do veículo.");

        // Cliente por CPF (cria ou atualiza)
        var cpf = Regex.Replace(dto.CpfCliente ?? "", @"\D", "");

        if (cpf.Length != 11) return BadRequest("CPF inválido (11 dígitos).");

        var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.CPF == cpf);

        if (cliente is null)
        {
            cliente = Cliente.Create(dto.NomeCliente, cpf, dto.TelefoneCliente);
            _db.Clientes.Add(cliente);
        }

        // Data da venda automática
        var dataVendaUtc = DateTime.UtcNow;

        // Protocolo
        static string GerarProtocolo20() => $"{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..4]}".ToUpper();

        string protocolo;
        do { protocolo = GerarProtocolo20(); }
        while (await _db.Vendas.IgnoreQueryFilters().AnyAsync(v => v.ProtocoloVenda == protocolo));

        // Criar venda 
        var venda = Venda.Create(
            veiculo.Id,
            concessionaria.Id,
            cliente.Id,
            dataVendaUtc,
            dto.PrecoVenda,
            protocolo
        );

        _db.Vendas.Add(venda);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = venda.Id },
            new { venda.Id, venda.ProtocoloVenda, venda.DataVenda });
    }

    // DELETE: api/vendas/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var v = await _db.Vendas.FindAsync(id);
        if (v is null) return NotFound();

        v.Delete();
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/vendas/deleted
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

    // POST: api/vendas/{id}/restore
    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var v = await _db.Vendas.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        if (v is null) return NotFound();
        if (!v.IsDeleted) return BadRequest("Item não está deletado.");

        v.Restore();
        await _db.SaveChangesAsync();
        return NoContent();
    }
}