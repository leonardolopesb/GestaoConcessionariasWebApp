using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Vendas.Create
{
    public class CreateVendaValidator : AbstractValidator<CreateVendaDto>
    {
        public CreateVendaValidator()
        {
            RuleFor(x => x.FabricanteNomeOuModeloVeiculo)
                .NotEmpty()
                .WithMessage("Selecione o veículo por modelo ou fabricante.");

            RuleFor(x => x.ConcessionariaNomeOuLocalizacao)
                .NotEmpty()
                .WithMessage("Selecione a concessionária por nome ou localização.");

            RuleFor(x => x.NomeCliente)
                .NotEmpty()
                .WithMessage("Nome do cliente é obrigatório.")
                .MaximumLength(100)
                .WithMessage("Nome do cliente deve ter no máximo 100 caracteres.");

            RuleFor(x => x.CpfCliente)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Matches(@"^\d{11}$").WithMessage("CPF deve conter 11 dígitos numéricos.");

            RuleFor(x => x.TelefoneCliente)
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .Matches(@"^[0-9()\-\s+]{8,20}$")
                .WithMessage("Telefone inválido (8 a 20 dígitos).");

            RuleFor(x => x.DataVenda)
                .NotEmpty().WithMessage("A data da venda é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data da venda não pode ser futura.");

            RuleFor(x => x.PrecoVenda)
                .GreaterThan(0).WithMessage("Preço de venda deve ser maior que 0.");
        }
    }
}
