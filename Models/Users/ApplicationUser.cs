using Microsoft.AspNetCore.Identity;

namespace GestaoConcessionariasWebApp.Models.Users;

public class ApplicationUser : IdentityUser
{
    public string NomeUsuario { get; set; } = null!;
    public AccessLevel AccessLevel { get; set; }

    public bool IsDeleted { get; private set; }
    public void Delete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}
