namespace GestaoConcessionariasWebApp.Models.Vendas.Create
{
    public sealed class VendaCreatedDto
    {
        public Guid Id { get; set; }
        public string ProtocoloVenda { get; set; } = default!;
    }
}
