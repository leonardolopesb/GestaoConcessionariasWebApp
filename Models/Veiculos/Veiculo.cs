using GestaoConcessionariasWebApp.Models.Fabricantes;
using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Veiculos
{
    public class Veiculo
    {
        [Key]
        public Guid Id { get; private set; }

        // LEMBRAR DE RETIRAR OS ATRIBUTOS DO MODEL E IMPLEMENTAR CÓDIGO BRUTO NA VALIDAÇÃO DE DADOS
        [Required, StringLength(100)]
        public string Modelo { get; private set; } = null!;

        [StringLength(4), Range(1950, 2025)]
        public string AnoFabricacao { get; private set; } = null!;

        [Range(0.01, 1000000.00)]
        public decimal Preco { get; private set; }

        [Required]
        public Guid FabricanteId { get; private set; }

        public Fabricante Fabricante { get; private set; } = null!;

        public TipoVeiculo TipoVeiculo { get; private set; }

        [StringLength(500)]
        public string? Descricao { get; private set; }

        public bool IsDeleted { get; private set; }

        protected Veiculo() { }

        private Veiculo(Guid id, string modelo, string anoFabricacao, decimal preco, Guid fabricanteId, TipoVeiculo tipoVeiculo, string? descricao)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Modelo = modelo;
            AnoFabricacao = anoFabricacao;
            Preco = preco;
            FabricanteId = fabricanteId;
            TipoVeiculo = tipoVeiculo;
            Descricao = descricao;
            IsDeleted = false;
        }

        public static Veiculo Create(string modelo, string anoFabricacao, decimal preco, Guid fabricanteId, TipoVeiculo tipoVeiculo, string? descricao)
        {
            return new Veiculo(Guid.NewGuid(), modelo, anoFabricacao, preco, fabricanteId, tipoVeiculo, descricao);
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
