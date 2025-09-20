namespace GestaoConcessionariasWebApp.Models.Clientes;

public class Cliente
{
    public Guid Id { get; private set; }

    public string Nome { get; private set; } = null!;

    public string CPF { get; private set; } = null!;

    public string Telefone { get; private set; } = null!;

    public bool IsDeleted { get; private set; }

    // Construtor protegido pro EF
    protected Cliente() { }

    // Construtor privado para uso interno
    private Cliente(Guid id, string nome, string cpf, string telefone)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Update(nome, cpf, telefone);
        IsDeleted = false;
    }

    public static Cliente Create(string nome, string cpf, string telefone)
    {
        return new Cliente(Guid.NewGuid(), nome, cpf, telefone);
    }

    public void Update(string nome, string cpf, string telefone)
    {
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
    }

    public void Delete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}
