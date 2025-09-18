using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Users.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("auth")]
// [Authorize]
[AllowAnonymous]
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
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!await _role.RoleExistsAsync(dto.AccessLevel.ToString()))
            return BadRequest("Nível de acesso inválido.");

        var user = new ApplicationUser
        {
            NomeUsuario = dto.FullName,
            AccessLevel = dto.AccessLevel,
            UserName = dto.UserName,
            Email = dto.Email,
            EmailConfirmed = true
        };

        var res = await _user.CreateAsync(user, dto.Password);
        if (!res.Succeeded) return BadRequest(res.Errors);

        await _user.AddToRoleAsync(user, dto.AccessLevel.ToString());

        return Ok(new { user.Id, user.UserName, Role = dto.AccessLevel.ToString() });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _user.FindByNameAsync(dto.UserName);
        if (user is null) return Unauthorized("Credenciais inválidas.");

        var deleted = await _user.Users
            .IgnoreQueryFilters()
            .AnyAsync(u => u.Id == user.Id && u.IsDeleted);
        if (deleted) return Unauthorized("Usuário desativado.");

        var res = await _signIn.PasswordSignInAsync(dto.UserName, dto.Password, dto.RememberMe, false);
        if (!res.Succeeded) return Unauthorized("Credenciais inválidas.");

        return Ok("Login realizado.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return Ok("Logout realizado.");
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var user = await _user.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await _user.GetRolesAsync(user);
        return Ok(new { user.Id, user.UserName, user.Email, user.NomeUsuario, user.AccessLevel, Roles = roles });
    }
}