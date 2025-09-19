using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Vendas.Create
{
    public sealed class CreateVendaDto
    {
        public string ConcessionariaNomeOuLocalizacao { get; set; } = null!;

        public string FabricanteNomeOuModeloVeiculo { get; set; } = null!;

        public string NomeCliente { get; set; } = null!;

        public string CpfCliente { get; set; } = null!;

        public string TelefoneCliente { get; set; } = null!;

        public DateTime DataVenda { get; set; }

        public decimal PrecoVenda { get; set; }

    }
}
