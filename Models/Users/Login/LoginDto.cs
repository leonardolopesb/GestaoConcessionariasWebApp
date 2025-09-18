namespace GestaoConcessionariasWebApp.Models.Users.Login;

public sealed class LoginDto
{
    public string NomeUsuario { get; set; } = default!;
    public string Password { get; set; } = default!;
}
