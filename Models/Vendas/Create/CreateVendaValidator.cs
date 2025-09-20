using FluentValidation;
using System.Text.RegularExpressions;

namespace GestaoConcessionariasWebApp.Models.Vendas.Create;

public sealed class CreateVendaValidator : AbstractValidator<CreateVendaDto>
{
    public CreateVendaValidator()
    {
        // Concessionária
        RuleFor(x => x.ConcessionariaNomeOuLocalizacao)
            .NotEmpty()
            .WithMessage("Nome ou localização da concessionária é obrigatório.")
            .MaximumLength(255);

        // Fabricante
        RuleFor(x => x.FabricanteNome)
            .NotEmpty()
            .WithMessage("Nome do fabricante é obrigatório.")
            .MaximumLength(100);

        // Modelo
        RuleFor(x => x.VeiculoModelo)
            .NotEmpty()
            .WithMessage("Modelo do veículo é obrigatório.")
            .MaximumLength(100);

        // Cliente
        RuleFor(x => x.NomeCliente)
            .NotEmpty()
            .WithMessage("Nome do cliente é obrigatório.")
            .MaximumLength(100);

        // CPF do cliente com valiidação
        RuleFor(x => x.CpfCliente)
            .NotEmpty()
            .WithMessage("CPF é obrigatório.")
            .Must(cpf =>
            {
                var digits = Regex.Replace(cpf ?? "", @"\D", "");
                return digits.Length == 11;
            })
            .WithMessage("CPF inválido. Informe 11 dígitos.");

        // Telefone do cliente com validação
        RuleFor(x => x.TelefoneCliente)
            .NotEmpty()
            .WithMessage("Telefone é obrigatório.")
            .Matches(@"^[0-9()\-\s+]{8,20}$")
            .WithMessage("Telefone inválido.");

        // Venda com validação de data futura
        RuleFor(x => x.DataVenda)
            .Must(d => d <= DateTime.UtcNow.AddMinutes(1))
            .WithMessage("Data da venda não pode ser futura.");

        // Preço da venda
        RuleFor(x => x.PrecoVenda)
            .GreaterThan(0M)
            .WithMessage("Preço da venda deve ser maior que 0.");
    }
}
