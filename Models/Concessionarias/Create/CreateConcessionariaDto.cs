using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Concessionarias.Create
{
    public class CreateConcessionariaDto
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = default!;

        [StringLength(10)]
        public string CEP { get; set; } = string.Empty;

        [StringLength(50)]
        public string Estado { get; set; } = string.Empty;

        [StringLength(50)]
        public string Cidade { get; set; } = string.Empty;

        [StringLength(50)]
        public string Rua { get; set; } = string.Empty;

        [StringLength(50)]
        public string NumeroDaRua { get; set; } = string.Empty;

        [StringLength(50)]
        public string? PontoReferencia { get; set; }

        [Phone, StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int CapacidadeMaximaVeiculos { get; set; }
    }


}
