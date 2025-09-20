using GestaoConcessionariasWebApp.Models.Clientes;
using GestaoConcessionariasWebApp.Models.Concessionarias;
using GestaoConcessionariasWebApp.Models.Veiculos;

namespace GestaoConcessionariasWebApp.Models.Vendas
{
    public class Venda
    {
        public Guid Id { get; private set; }

        public Guid VeiculoId { get; private set; }

        public Guid ConcessionariaId { get; private set; }

        public Guid ClienteId { get; private set; }

        public DateTime DataVenda { get; private set; }

        public decimal PrecoVenda { get; private set; }

        public string ProtocoloVenda { get; private set; } = null!;

        public bool IsDeleted { get; private set; }


        public Veiculo Veiculo { get; private set; } = default!;
        public Concessionaria Concessionaria { get; private set; } = default!;
        public Cliente Cliente { get; private set; } = default!;

        // Construtor protegido para EF
        protected Venda() { }

        // Construtor privado para uso interno
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

        public void Update(
            Guid veiculoId,
            Guid concessionariaId,
            Guid clienteId,
            DateTime data,
            decimal preco)
        {
            VeiculoId = veiculoId;
            ConcessionariaId = concessionariaId;
            ClienteId = clienteId;
            DataVenda = data;
            PrecoVenda = preco;
        }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
