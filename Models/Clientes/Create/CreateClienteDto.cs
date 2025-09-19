using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Clientes.Create
{
    public class CreateClienteDto
    {
        public string Nome { get; set; } = null!;

        public string CPF { get; set; } = null!;

        public string Telefone { get; set; } = null!;
    }

}
