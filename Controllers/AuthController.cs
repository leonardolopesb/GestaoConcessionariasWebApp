using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Users.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly UserManager<ApplicationUser> _users;

    public AuthController(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> users)
    {
        _signIn = signIn;
        _users = users;
    }

    public record RegisterDto(string Email, string Password, string NomeUsuario, string NivelAcesso);
    public record LoginDto(string Email, string Password);

    [HttpPost("register")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var role = dto.NivelAcesso?.Trim().ToLower() switch
        {
            "administrador" => Roles.Admin,
            "gerente" => Roles.Gerente,
            "vendedor" => Roles.Vendedor,
            _ => Roles.Vendedor
        };

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            EmailConfirmed = true,
            NomeUsuario = dto.NomeUsuario,
            NivelAcesso = role
        };

        var res = await _users.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        await _users.AddToRoleAsync(user, role);
        return Created("", new { user.Id, user.Email, Role = role });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _signIn.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        return result.Succeeded ? NoContent() : Unauthorized();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return NoContent();
    }
}