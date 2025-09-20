using GestaoConcessionariasWebApp.Models.Fabricantes;
using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Veiculos
{
    public class Veiculo
    {
        public Guid Id { get; private set; }

        public string Modelo { get; private set; } = null!;

        public int AnoFabricacao { get; private set; }

        public decimal Preco { get; private set; }

        public Guid FabricanteId { get; private set; }

        public Fabricante Fabricante { get; private set; } = null!;

        public TipoVeiculo TipoVeiculo { get; private set; }

        public string? Descricao { get; private set; }

        public bool IsDeleted { get; private set; }

        // Construtor protegido para EF
        protected Veiculo() { }

        // Construtor privado para uso interno
        private Veiculo(Guid id, string modelo, int anoFabricacao, decimal preco, Guid fabricanteId, TipoVeiculo tipoVeiculo, string? descricao)
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

        public static Veiculo Create(string modelo, int anoFabricacao, decimal preco, Guid fabricanteId, TipoVeiculo tipoVeiculo, string? descricao)
        {
            return new Veiculo(Guid.NewGuid(), modelo, anoFabricacao, preco, fabricanteId, tipoVeiculo, descricao);
        }

        // Update
        public void Update (
            string modelo,
            int anoFabricacao, 
            decimal preco,
            Guid fabricanteId, 
            TipoVeiculo tipoVeiculo,
            string? descricao)
        {
            Modelo = modelo;
            AnoFabricacao = anoFabricacao;
            Preco = preco;
            FabricanteId = fabricanteId;
            TipoVeiculo = tipoVeiculo;
            Descricao = descricao;
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
