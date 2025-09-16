using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Fabricantes.Create
{
    public class CreateFabricanteDto
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = default!;

        [StringLength(50)]
        public string PaisOrigem { get; set; } = default!;

        [Range(1800, 2100)]
        public int AnoFundacao { get; set; }

        [Url, StringLength(255)]
        public string Website { get; set; } = default!;
    }
}
