using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GestaoConcessionariasWebApp.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string? NomeUsuario { get; set; }

        public string? NivelAcesso { get; set; }
    }
}
