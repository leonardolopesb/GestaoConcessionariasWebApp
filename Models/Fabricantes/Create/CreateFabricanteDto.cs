using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Fabricantes.Create
{
    public class CreateFabricanteDto
    {
        [Required, StringLength(100)]
        public string NomeFabricante { get; set; } = null!;

        [StringLength(50)]
        public string PaisOrigem { get; set; } = null!;

        [Range(1800, 2025)]
        public string AnoFundacao { get; set; } = null!;

        [Url, StringLength(255)]
        public string Website { get; set; } = null!;
    }
}
