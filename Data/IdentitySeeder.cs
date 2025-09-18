using GestaoConcessionariasWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GestaoConcessionariasWebApp.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
        var role = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var user = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        // Cria as roles do nível de acesso
        string[] roles = { "Admin", "Vendedor", "Gerente" };
        foreach (var r in roles)
            if (!await role.RoleExistsAsync(r))
                await role.CreateAsync(new IdentityRole(r));

        // Cria o usuário admin de acordo com as configurações protegidas no dotnet user-secrets
        var adminEmail = configuration["ADMIN_EMAIL"];
        var adminUserName = configuration["ADMIN_USERNAME"];
        var adminPassword = configuration["ADMIN_PASSWORD"];

        // Utiliza credenciais padronizados apenas em ambiente de desenvolvimento
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminUserName) || string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogInformation("Existem credenciais ausentes. Preencha-os!");
            return;
        }

        // Cria um novo admin caso não exista ainda
        var admin = await user.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminUserName,
                NomeUsuario = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
                AccessLevel = AccessLevel.Admin
            };

            var create = await user.CreateAsync(admin, adminPassword);

            if (!create.Succeeded)
            {
                var error = string.Join("; ", create.Errors.Select(e => e.Description));
                logger.LogError("Falha ao criar admin: {error}", error);
                return;
            }

            await user.AddToRoleAsync(admin, "Admin");
            logger.LogInformation("Admin criado: {email}", adminEmail);
        }
        else
        {
            if (!await user.IsInRoleAsync(admin, "Admin"))
                await user.AddToRoleAsync(admin, "Admin");
        }
    }
}
