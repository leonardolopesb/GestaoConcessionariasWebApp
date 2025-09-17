namespace GestaoConcessionariasWebApp.Models.Vendas.List
{
    public sealed class VendaListItemDto
    {
        public Guid Id { get; set; }
        public string Protocolo { get; set; } = default!;
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
        public string VeiculoModelo { get; set; } = default!;
        public string ConcessionariaNome { get; set; } = default!;
        public string ClienteNome { get; set; } = default!;
        public string ClienteCpf { get; set; } = default!;
    }
}
