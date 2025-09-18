using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        // Nome do usuário (em português) para evitar conflito com UserName do Identity
        [Required, StringLength(50)]
        public string NomeUsuario { get; set; } = null!;

        public AccessLevel AccessLevel { get; set; }

        public bool IsDeleted { get; private set; }

        public void Delete() => IsDeleted = true;
        public void Restore() => IsDeleted = false;
    }
}
