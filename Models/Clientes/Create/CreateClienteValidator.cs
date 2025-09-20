using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Clientes.Create;

public class CreateClienteValidator : AbstractValidator<CreateClienteDto>
{
    public CreateClienteValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CPF)
            .NotEmpty()
            .WithMessage("O CPF é obrigatório.")
            .Matches(@"^\d{11}$")
            .WithMessage("CPF inválido. Deve conter somente números (sem hífen e sem ponto) e exatamente 11 dígitos.");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .WithMessage("O telefone é obrigatório.")
            .Matches(@"^[0-9\s()+\-]{8,15}$")
            .WithMessage("Telefone inválido. Deve ter entre 8 e 15 caracteres.");
    }
}
