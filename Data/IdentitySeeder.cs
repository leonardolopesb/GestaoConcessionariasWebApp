using GestaoConcessionariasWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace GestaoConcessionariasWebApp.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var role = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var user = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = { "Admin", "Vendedor", "Gerente" };

        foreach (var r in roles)
            if (!await role.RoleExistsAsync(r))
                await role.CreateAsync(new IdentityRole(r));

        var adminEmail = "admin@local.com";
        var admin = await user.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                NomeUsuario = "Administrador do Sistema",
                AccessLevel = AccessLevel.Admin,
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            await user.CreateAsync(admin, "Admin@123");
            await user.AddToRoleAsync(admin, "Admin");
        }
    }
}