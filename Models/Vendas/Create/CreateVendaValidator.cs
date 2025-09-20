using FluentValidation;
using System.Text.RegularExpressions;

namespace GestaoConcessionariasWebApp.Models.Vendas.Create;

public sealed class CreateVendaValidator : AbstractValidator<CreateVendaDto>
{
    public CreateVendaValidator()
    {
        // Nome ou localização da concessionária
        RuleFor(x => x.ConcessionariaNomeOuLocalizacao)
            .NotEmpty()
            .WithMessage("Nome ou localização da concessionária é obrigatório.")
            .MaximumLength(255);

        // Nome do fabricante
        RuleFor(x => x.FabricanteNome)
            .NotEmpty()
            .WithMessage("Nome do fabricante é obrigatório.")
            .MaximumLength(100);

        // Modelo do veículo
        RuleFor(x => x.VeiculoModelo)
            .NotEmpty()
            .WithMessage("Modelo do veículo é obrigatório.")
            .MaximumLength(100);

        // Nome do cliente
        RuleFor(x => x.NomeCliente)
            .NotEmpty()
            .WithMessage("Nome completo do cliente é obrigatório.")
            .MaximumLength(100);

        // CPF do cliente
        RuleFor(x => x.CpfCliente)
            .NotEmpty()
            .WithMessage("CPF é obrigatório.")
            .Matches(@"^\d{11}$")
            .WithMessage("CPF inválido. Deve conter somente números (sem hífen e sem ponto) e exatamente 11 dígitos.");

        // Telefone do cliente
        RuleFor(x => x.TelefoneCliente)
            .NotEmpty()
            .WithMessage("Telefone é obrigatório.")
            .Matches(@"^[0-9()\-\s+]{8,15}$")
            .WithMessage("Telefone inválido. Deve ter entre 8 e 15 caracteres.");

        // Venda com validação para data não futura
        RuleFor(x => x.DataVenda)
            .Must(d => d <= DateTime.UtcNow.AddMinutes(1))
            .WithMessage("Data da venda não pode ser futura.");

        // Preço da venda
        RuleFor(x => x.PrecoVenda)
            .GreaterThan(0M)
            .WithMessage("Preço da venda deve ser maior que 0.");
    }
}
