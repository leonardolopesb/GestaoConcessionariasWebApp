using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Concessionarias.Create;


public class CreateConcessionariaValidator : AbstractValidator<CreateConcessionariaDto>
{
    public CreateConcessionariaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("O Nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O Nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.CEP)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório.")
            .Matches(@"^\d{8}$")
            .WithMessage("CEP inválido. Use apenas 8 dígitos (ex: 01001000).");

        RuleFor(x => x.Estado)
            .NotEmpty()
            .WithMessage("O Estado é obrigatório.")
            .Length(50);

        RuleFor(x => x.Cidade)
            .NotEmpty()
            .WithMessage("A Cidade é obrigatória.")
            .MaximumLength(50)
            .WithMessage("A Cidade deve ter no máximo 50 caracteres.");

        RuleFor(x => x.Endereco)
            .NotEmpty()
            .WithMessage("O Endereço é obrigatório.")
            .MaximumLength(255)
            .WithMessage("O Endereço deve ter no máximo 255 caracteres.");

        RuleFor(x => x.Telefone)
            .Matches(@"^[0-9\s()+\-]{8,20}$")
            .When(x => !string.IsNullOrWhiteSpace(x.Telefone))
            .WithMessage("Telefone inválido.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("E-mail inválido.")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.CapacidadeMaximaVeiculos)
            .GreaterThan(0)
            .WithMessage("Capacidade deve ser maior que 0.");
    }
}
