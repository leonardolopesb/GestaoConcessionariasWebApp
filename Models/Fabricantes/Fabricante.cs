using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Fabricantes
{
    public class Fabricante
    {
        [Key]
        public Guid Id { get; private set; }

        [Required, StringLength(100)]
        public string Nome { get; private set; } = default!;

        [StringLength(50)]
        public string PaisOrigem { get; private set; }

        [Range(1800, 2100)]
        public int AnoFundacao { get; private set; }

        [Url, StringLength(255)]
        public string Website { get; private set; }

        public bool IsDeleted { get; private set; } = false;

        private Fabricante(Guid id, string nome, string paisOrigem, int anoFundacao, string website)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Nome = nome;
            PaisOrigem = paisOrigem;
            AnoFundacao = anoFundacao;
            Website = website;
            IsDeleted = false;
        }

        public static Fabricante Create(string nome, string paisOrigem, int anoFundacao, string website)
        {
            return new Fabricante(Guid.NewGuid(), nome, paisOrigem, anoFundacao, website);
        }

        // Soft delete por segurança
        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
