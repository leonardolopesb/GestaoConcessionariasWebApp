using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Fabricantes.Create
{
    public class CreateFabricanteDto
    {
        public string NomeFabricante { get; set; } = null!;

        public string PaisOrigem { get; set; } = null!;

        public int AnoFundacao { get; set; }

        public string Website { get; set; } = null!;
    }
}
