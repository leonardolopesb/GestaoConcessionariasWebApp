using GestaoConcessionariasWebApp.Models.Clientes;
using GestaoConcessionariasWebApp.Models.Concessionarias;
using GestaoConcessionariasWebApp.Models.Veiculos;
using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Vendas
{
    public class Venda
    {
        [Key] public Guid Id { get; private set; }

        [Required] public Guid VeiculoId { get; private set; }
        [Required] public Guid ConcessionariaId { get; private set; }
        [Required] public Guid ClienteId { get; private set; }

        [Required] public DateTime DataVenda { get; private set; }

        [Required] public decimal PrecoVenda { get; private set; }

        [Required, StringLength(20)]
        public string ProtocoloVenda { get; private set; } = default!;

        public bool IsDeleted { get; private set; }

        // APAGAR EM BREVE AS TRÊS VARIÁVEIS
        public Veiculo Veiculo { get; private set; } = default!;
        public Concessionaria Concessionaria { get; private set; } = default!;
        public Cliente Cliente { get; private set; } = default!;

        protected Venda() { }

        private Venda(Guid id, Guid veiculoId, Guid concessionariaId, Guid clienteId, DateTime data, decimal preco, string protocolo)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            VeiculoId = veiculoId;
            ConcessionariaId = concessionariaId;
            ClienteId = clienteId;
            DataVenda = data;
            PrecoVenda = preco;
            ProtocoloVenda = protocolo;
            IsDeleted = false;
        }

        public static Venda Create(Guid veiculoId, Guid concessionariaId, Guid clienteId, DateTime data, decimal preco, string protocolo)
        {
            return new Venda(Guid.NewGuid(), veiculoId, concessionariaId, clienteId, data, preco, protocolo);
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
