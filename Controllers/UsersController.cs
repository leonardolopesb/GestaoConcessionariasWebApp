using GestaoConcessionariasWebApp.Models.Fabricantes;
using GestaoConcessionariasWebApp.Models.Users;
using GestaoConcessionariasWebApp.Models.Users.Register;
using GestaoConcessionariasWebApp.Models.Users.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoConcessionariasWebApp.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var user = await _userManager.Users
            .Select(u => new {
                u.Id, 
                u.NomeUsuario,
                u.Email, 
                NivelAcesso = u.AccessLevel.ToString() 
            })
            .ToListAsync();

        return Ok(user);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        return user is null ? NotFound() : Ok(new {
            user.Id,
            user.NomeUsuario,
            user.Email,
            user.AccessLevel
        });
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        if (await _userManager.FindByNameAsync(dto.NomeUsuario) is not null)
            return Conflict("Já existe um usuário com este nome de usuário.");

        if (await _userManager.FindByEmailAsync(dto.Email) is not null)
            return Conflict("Já existe um usuário com este e-mail.");

        var user = new ApplicationUser
        {
            UserName = dto.NomeUsuario,
            NomeUsuario = dto.NomeUsuario,
            Email = dto.Email,
            EmailConfirmed = true,
            AccessLevel = dto.AccessLevel
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, dto.AccessLevel.ToString());

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new
        {
            user.Id,
            user.NomeUsuario,
            user.Email,
            AccessLevel = user.AccessLevel.ToString()
        });
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        user.NomeUsuario = dto.NomeUsuario;
        user.UserName = dto.NomeUsuario;
        user.Email = dto.Email;
        user.AccessLevel = dto.AccessLevel;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(
            new {
                user.Id, 
                user.NomeUsuario,
                user.Email,
                AccessLevel = user.AccessLevel.ToString()
            });
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        user.Delete();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return NoContent();
    }

    // GET: api/users/deleted
    [HttpGet("deleted")]
    public async Task<IActionResult> GetDeleted()
    {
        var itens = await _userManager.Users
            .IgnoreQueryFilters()
            .Where(u => u.IsDeleted)
            .OrderBy(u => u.NomeUsuario)
            .Select(u => new {
                u.Id,
                u.NomeUsuario,
                u.Email,
                u.AccessLevel,
                u.IsDeleted
            })
            .ToListAsync();

        return Ok(itens);
    }

    // POST: api/users/{id}/restore
    [HttpPost("{id}/restore")]
    public async Task<IActionResult> Restore(string id)
    {
        var user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) 
            return NotFound();

        if (!user.IsDeleted) 
            return BadRequest("Usuário não está deletado.");

        user.Restore();

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded) 
            return BadRequest(result.Errors);

        return NoContent();
    }
}