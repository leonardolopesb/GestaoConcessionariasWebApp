using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Users.Login;

public sealed class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.NomeUsuario)
            .NotEmpty()
            .WithMessage("O nome de usuário é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O nome de usuário não pode ter mais de 50 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .MinimumLength(6)
            .WithMessage("A senha deve ter no mínimo 6 caracteres.")
            .MaximumLength(255)
            .WithMessage("A senha deve ter no máximo 255 caracteres.");
    }
}