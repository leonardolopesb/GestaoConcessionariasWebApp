namespace GestaoConcessionariasWebApp.Models.Vendas.List
{
    public sealed class VendaListItemDto
    {
        // Dados que irão para a tabela de listagem de vendas na UI /tabelas/vendas/vendas.html
        public Guid Id { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid ConcessionariaId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
        public string Protocolo { get; set; } = default!;
    }
}
