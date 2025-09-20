namespace GestaoConcessionariasWebApp.Models.Fabricantes;

public class Fabricante
{
    public Guid Id { get; private set; }

    public string NomeFabricante { get; private set; } = null!;

    public string PaisOrigem { get; private set; } = null!;

    public int AnoFundacao { get; private set; }

    public string Website { get; private set; } = null!;

    public bool IsDeleted { get; private set; } = false;

    // Construtor protegido para EF
    protected Fabricante() { }

    // Construtor privado para uso interno
    private Fabricante(Guid id, string nomeFabricante, string paisOrigem, int anoFundacao, string website)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        NomeFabricante = nomeFabricante;
        PaisOrigem = paisOrigem;
        AnoFundacao = anoFundacao;
        Website = website;
        IsDeleted = false;
    }

    public static Fabricante Create(string nomeFabricante, string paisOrigem, int anoFundacao, string website)
    {
        return new Fabricante(Guid.NewGuid(), nomeFabricante, paisOrigem, anoFundacao, website);
    }

    public void Update(
        string nomeFabricante,
        string paisOrigem,
        int anoFundacao,
        string website)
    {
        NomeFabricante = nomeFabricante;
        PaisOrigem = paisOrigem;
        AnoFundacao = anoFundacao;
        Website = website;
    }

    // Soft Delete (por segurança) + Restore
    public void Delete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}
