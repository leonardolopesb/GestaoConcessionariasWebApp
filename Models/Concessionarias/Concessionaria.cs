namespace GestaoConcessionariasWebApp.Models.Concessionarias;

public class Concessionaria
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; } = default!;

    public string CEP { get; private set; } = null!;

    public string Cidade { get; private set; } = null!;

    public string Estado { get; private set; } = null!;

    public string Endereco { get; private set; } = null!;

    public string Telefone { get; private set; } = null!;

    public string Email { get; private set; } = null!;

    public int CapacidadeMaximaVeiculos { get; private set; }

    public bool IsDeleted { get; private set; }

    protected Concessionaria() { }

    private Concessionaria(
        Guid id,
        string nome,
        string cep,
        string cidade,
        string estado,
        string endereco,
        string telefone,
        string email,
        int capacidade)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Nome = nome;
        CEP = cep;
        Cidade = cidade;
        Estado = estado;
        Endereco = endereco;
        Telefone = telefone;
        Email = email;
        CapacidadeMaximaVeiculos = capacidade;
        IsDeleted = false;
    }

    public static Concessionaria Create(
        string nome,
        string cep,
        string cidade,
        string estado,
        string endereco,
        string telefone,
        string email,
        int capacidade)
    {
        return new Concessionaria(Guid.NewGuid(), nome, cep, cidade, estado, endereco, telefone, email, capacidade);
    }

    public void Update(
        string nome,
        string cep,
        string cidade,
        string estado,
        string endereco,
        string telefone,
        string email,
        int capacidade)
    {
        Nome = nome;
        CEP = cep;
        Cidade = cidade;
        Estado = estado;
        Endereco = endereco;
        Telefone = telefone;
        Email = email;
        CapacidadeMaximaVeiculos = capacidade;
    }

    public void Delete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}
