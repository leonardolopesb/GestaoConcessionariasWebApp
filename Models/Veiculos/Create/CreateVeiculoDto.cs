using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Veiculos.Create
{
    public class CreateVeiculoDto
    {
        [Required, StringLength(100)]
        public string Modelo { get; set; } = null!;

        [StringLength(4), Range(1950, 2025)]
        public string AnoFabricacao { get; set; } = null!;

        [Range(0.01, 1000000.00)]
        public decimal Preco { get; set; }

        [Required]
        public Guid FabricanteId { get; set; }

        public TipoVeiculo TipoVeiculo { get; set; }

        [StringLength(500)]
        public string? Descricao { get; set; }
    }
}
