namespace GestaoConcessionariasWebApp.Models.Vendas.Create
{
    public sealed class CreateVendaDto
    {
        // Concessionária: permitir busca por nome [que é único] ou localização
        public string ConcessionariaNomeOuLocalizacao { get; set; } = string.Empty;

        // Fabricante: permitir busca por nome [que é único] porém não armazena, sua função é apenas buscar o veículo
        public string FabricanteNome { get; set; } = string.Empty;

        // Veículo: permitir busca por modelo, este que vem do fabricante selecionado
        public string VeiculoModelo { get; set; } = string.Empty;

        // Dados do cliente
        public string NomeCliente { get; set; } = string.Empty;
        public string CpfCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;

        // Dados da venda
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
    }
}
