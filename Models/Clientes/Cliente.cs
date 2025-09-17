using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GestaoConcessionariasWebApp.Models.Clientes
{
    public class Cliente
    {
        [Key] public Guid Id { get; private set; }

        [Required, StringLength(100)]
        public string Nome { get; private set; } = default!;

        [Required, StringLength(11, MinimumLength = 11)]
        public string CPF { get; private set; } = default!;

        [Phone, StringLength(20)]
        public string? Telefone { get; private set; }

        public bool IsDeleted { get; private set; }

        protected Cliente() { }

        private Cliente(Guid id, string nome, string cpf, string? telefone)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Update(nome, cpf, telefone);
            IsDeleted = false;
        }

        public static Cliente Create(string nome, string cpf, string? telefone)
        {
            return new Cliente(Guid.NewGuid(), nome, Regex.Replace(cpf, @"\D", ""), telefone);
        }

        public void Update(string nome, string cpf, string? telefone)
        {
            Nome = nome;
            CPF = cpf;
            Telefone = telefone;
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
