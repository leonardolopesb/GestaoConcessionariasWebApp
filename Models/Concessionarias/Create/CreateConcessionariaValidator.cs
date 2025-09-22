using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Concessionarias.Create;

public class CreateConcessionariaValidator : AbstractValidator<CreateConcessionariaDto>
{
    public CreateConcessionariaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CEP)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{10}$")
            .WithMessage("CEP inválido. Use apenas 8 dígitos (ex: 51140235).");

        RuleFor(x => x.Cidade)
            .NotEmpty()
            .WithMessage("A cidade é obrigatória.")
            .MaximumLength(50)
            .WithMessage("A cidade deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("O estado é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O estado deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Endereco)
            .NotEmpty()
            .WithMessage("O endereço é obrigatório.")
            .MaximumLength(255)
            .WithMessage("O endereço deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Telefone)
            .NotEmpty()
            .WithMessage("O Telefone é obrigatório.")
            .Matches(@"^[0-9\s()+\-]{8,15}$")
            .WithMessage("Telefone inválido.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O E-mail é obrigatório.")
            .EmailAddress()
            .WithMessage("E-mail inválido.")
            .MaximumLength(100)
            .WithMessage("E-mail deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CapacidadeMaximaVeiculos)
            .GreaterThan(0)
            .WithMessage("A capacidade máxima deve possuir um número inteiro positivo.");
    }
}
