using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Users.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _user;
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly RoleManager<IdentityRole> _role;

    public AuthController(
        UserManager<ApplicationUser> user,
        SignInManager<ApplicationUser> signIn,
        RoleManager<IdentityRole> role)
    {
        _user = user;
        _signIn = signIn;
        _role = role;
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        // Condição para que somente admins possam criar novos usuários
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (!await _role.RoleExistsAsync(dto.AccessLevel.ToString()))
            return BadRequest("Nível de acesso inválido.");

        var user = new ApplicationUser
        {
            UserName = dto.NomeUsuario,
            NomeUsuario = dto.NomeUsuario,
            Email = dto.Email,
            AccessLevel = dto.AccessLevel,
            EmailConfirmed = true
        };

        var res = await _user.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        await _user.AddToRoleAsync(user, dto.AccessLevel.ToString());

        return CreatedAtAction(nameof(Me), new { }, new
        {
            user.Id,
            user.NomeUsuario,
            user.Email,
            AccessLevel = user.AccessLevel.ToString()
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        ApplicationUser? user = await _user.FindByNameAsync(dto.NomeUsuario);

        if (user is null)
            return Unauthorized("Credenciais inválidas.");

        var isDeleted = await _user.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Id == user.Id && u.IsDeleted);

        if (isDeleted)
            return Unauthorized("Usuário desativado.");

        var res = await _signIn.PasswordSignInAsync(user, dto.Password, isPersistent: false, lockoutOnFailure: false);

        if (!res.Succeeded)
            return Unauthorized("Credenciais inválidas.");

        return Ok("Login realizado.");
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return Ok("Logout realizado.");
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var user = await _user.GetUserAsync(User);
        if (user == null) return Unauthorized();

        return Ok(new
        {
            user.Id,
            user.NomeUsuario,
            user.Email,
            AccessLevel = user.AccessLevel.ToString()
        });
    }
}