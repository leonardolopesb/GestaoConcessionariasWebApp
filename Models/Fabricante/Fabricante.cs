using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models
{
    public class Fabricante
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(100)]
        public string Nome { get; set; } = default!;

        [StringLength(50)]
        public string PaisOrigem { get; set; } = null!;

        public string AnoFundacao { get; set; } = null!;

        [Url]
        public string Website { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
