using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Fabricantes
{
    public class Fabricante
    {
        [Key]
        public Guid Id { get; private set; }

        // LEMBRAR DE RETIRAR OS ATRIBUTOS DO MODEL E IMPLEMENTAR CÓDIGO BRUTO NA VALIDAÇÃO DE DADOS
        [Required, StringLength(100)]
        public string NomeFabricante { get; private set; } = null!;

        [StringLength(50)]
        public string PaisOrigem { get; private set; } = null!;

        [StringLength(4), Range(1950, 2025)]
        public string AnoFundacao { get; private set; } = null!;

        [Url, StringLength(255)]
        public string Website { get; private set; } = null!;

        public bool IsDeleted { get; private set; } = false;

        protected Fabricante() { }

        private Fabricante(Guid id, string nomeFabricante, string paisOrigem, string anoFundacao, string website)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            NomeFabricante = nomeFabricante;
            PaisOrigem = paisOrigem;
            AnoFundacao = anoFundacao;
            Website = website;
            IsDeleted = false;
        }

        public static Fabricante Create(string nomeFabricante, string paisOrigem, string anoFundacao, string website)
        {
            return new Fabricante(Guid.NewGuid(), nomeFabricante, paisOrigem, anoFundacao, website);
        }

        // Soft Delete (por segurança) + Restore
        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
