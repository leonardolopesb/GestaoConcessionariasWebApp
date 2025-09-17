using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Concessionarias
{
    public class Concessionaria
    {
        [Key]
        public Guid Id { get; private set; }

        // LEMBRAR DE RETIRAR OS ATRIBUTOS DO MODEL E IMPLEMENTAR CÓDIGO BRUTO NA VALIDAÇÃO DE DADOS
        [Required, StringLength(100)]
        public string Nome { get; private set; } = default!;

        [StringLength(10)]
        public string CEP { get; private set; } = null!;

        [StringLength(50)]
        public string Estado { get; private set; } = null!;

        [StringLength(50)]
        public string Cidade { get; private set; } = null!;

        [StringLength(50)]
        public string Rua { get; private set; } = null!;

        [StringLength(50)]
        public string NumeroDaRua { get; private set; } = null!;

        [StringLength(50)]
        public string? PontoReferencia { get; private set; }

        [Phone, StringLength(20)]
        public string Telefone { get; private set; } = null!;

        [EmailAddress, StringLength(100)]
        public string Email { get; private set; } = null!;

        [Range(0, int.MaxValue)]
        public int CapacidadeMaximaVeiculos { get; private set; }

        public bool IsDeleted { get; private set; }

        protected Concessionaria() { }

        private Concessionaria(
            Guid id,
            string nome,
            string cep,
            string estado,
            string cidade,
            string rua,
            string numero,
            string? referencia,
            string telefone,
            string email,
            int capacidade)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Nome = nome;
            CEP = cep;
            Estado = estado;
            Cidade = cidade;
            Rua = rua;
            NumeroDaRua = numero;
            PontoReferencia = referencia;
            Telefone = telefone;
            Email = email;
            CapacidadeMaximaVeiculos = capacidade;
            IsDeleted = false;
        }

        public static Concessionaria Create(
            string nome,
            string cep,
            string estado,
            string cidade,
            string rua,
            string numero,
            string? referencia,
            string telefone,
            string email,
            int capacidade)
        {
            return new Concessionaria(Guid.NewGuid(), nome, cep, estado, cidade, rua, numero, referencia, telefone, email, capacidade);
        }

        public void Update(
            string nome,
            string cep,
            string estado,
            string cidade,
            string rua,
            string numero,
            string? referencia,
            string telefone,
            string email,
            int capacidade)
        {
            Nome = nome;
            CEP = cep;
            Estado = estado;
            Cidade = cidade;
            Rua = rua;
            NumeroDaRua = numero;
            PontoReferencia = referencia;
            Telefone = telefone;
            Email = email;
            CapacidadeMaximaVeiculos = capacidade;
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
