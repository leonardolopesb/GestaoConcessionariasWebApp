using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Vendas.Create
{
    public sealed class CreateVendaDto
    {
        // TABELA QUE SERÁ PREENCHIDA NO FRONT
        public string ConcessionariaNome { get; set; } = default!;
        public string FabricanteNome { get; set; } = default!;
        public string VeiculoModelo { get; set; } = default!;
        

        // dados do cliente
        public string NomeCliente { get; set; } = default!;
        public string CpfCliente { get; set; } = default!;
        public string? TelefoneCliente { get; set; }

        // dados da venda
        [Required] public decimal PrecoVenda { get; set; }

        // o protocolo e a data da venda serão geradoo automaticamente 
    }
}
