namespace GestaoConcessionariasWebApp.Models.Users.Login;

public sealed class LoginDto
{
    public string NomeUsuario { get; set; } = null!;
    public string Password { get; set; } = null!;
}
