using GestaoConcessionariasWebApp.Data;
using GestaoConcessionariasWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder
    .Services.
    AddDbContext<ApplicationDbContext>(
        options =>options
        .UseSqlServer(connectionString));

builder
    .Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder
    .Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configura o cookie para retornar erro 4** ao invés de redirecionar
builder.Services.ConfigureApplicationCookie(o =>
{
    o.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
    o.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
});

builder
    .Services
    .AddControllers();

builder
    .Services
    .AddEndpointsApiExplorer();

builder
    .Services
    .AddSwaggerGen();

builder
    .Services
    .AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

await IdentitySeeder.SeedAsync(app.Services);

app.Run();
