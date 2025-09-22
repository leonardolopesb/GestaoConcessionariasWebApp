using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Users.Register;

public sealed class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.NomeUsuario)
            .NotEmpty()
            .WithMessage("O nome de usuário é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O nome de usuário não pode ter mais de 50 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório.")
            .EmailAddress()
            .WithMessage("O email informado não é válido. Utilize o formato: usuario@email.com")
            .MaximumLength(100)
            .WithMessage("O email não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória.")
            .MinimumLength(8)
            .WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .MaximumLength(255)
            .WithMessage("A senha deve ter no máximo 255 caracteres.")
            .Matches(@"[A-Z]")
            .WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]")
            .WithMessage("A senha deve conter pelo menos uma letra minúscula.")
            .Matches(@"\d")
            .WithMessage("A senha deve conter pelo menos um número.");


        RuleFor(x => x.AccessLevel)
            .IsInEnum()
            .WithMessage("O nível de acesso informado não é válido.");
    }
}
