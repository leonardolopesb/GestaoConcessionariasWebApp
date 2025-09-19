using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Veiculos.Create
{
    public class CreateVeiculoDto
    {
        public string Modelo { get; set; } = null!;

        public int AnoFabricacao { get; set; }

        public decimal Preco { get; set; }

        public Guid FabricanteId { get; set; }

        public TipoVeiculo TipoVeiculo { get; set; }

        public string? Descricao { get; set; }
    }
}
