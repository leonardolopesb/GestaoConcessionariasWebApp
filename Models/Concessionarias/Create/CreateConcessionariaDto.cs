using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Concessionarias.Create
{
    public class CreateConcessionariaDto
    {
        public string Nome { get; set; } = null!;

        public string Endereco { get; set; } = null!;

        public string Cidade { get; set; } = null!;

        public string Estado { get; set; } = null!;

        public string CEP { get; set; } = null!;

        public string Telefone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int CapacidadeMaximaVeiculos { get; set; }
    }
}
