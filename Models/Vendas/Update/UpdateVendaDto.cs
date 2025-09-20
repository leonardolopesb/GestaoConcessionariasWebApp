using GestaoConcessionariasWebApp.Models.Vendas.Create;

namespace GestaoConcessionariasWebApp.Models.Vendas.Update
{
    public class UpdateVendaDto
    {
        public Guid VeiculoId { get; set; }
        public Guid ConcessionariaId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal PrecoVenda { get; set; }
    }
}
