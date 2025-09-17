using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Clientes.Create
{
    public class CreateClienteDto
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = default!;

        [Required, StringLength(14)]
        public string CPF { get; set; } = default!;

        [StringLength(20)]
        public string? Telefone { get; set; }
    }

}
