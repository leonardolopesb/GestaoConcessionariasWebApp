using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Veiculos.Create
{
    public class CreateVeiculoValidator : AbstractValidator<CreateVeiculoDto>
    {
        public CreateVeiculoValidator()
        {
            RuleFor(x => x.Modelo)
                .NotEmpty()
                .WithMessage("O modelo é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O modelo deve ter no máximo 100 caracteres.");

            RuleFor(x => x.AnoFabricacao)
                .InclusiveBetween(0, DateTime.UtcNow.Year)
                .WithMessage($"O ano de fabricação deve ser menor do que o ano anual: {DateTime.UtcNow.Year}.");

            RuleFor(x => x.Preco)
                .GreaterThan(0m)
                .WithMessage("O preço deve ser um número maior do que zero.")
                .LessThan(100_000_000m)
                .WithMessage("O preço deve ser menor a 100 milhões."); ;

            RuleFor(x => x.FabricanteId)
                .NotEmpty()
                .WithMessage("Selecione um fabricante válido.");

            RuleFor(x => x.TipoVeiculo)
                .IsInEnum()
                .WithMessage("Tipo de veículo inválido.");

            RuleFor(x => x.Descricao)
                .MaximumLength(500)
                .WithMessage("A descrição deve ter no máximo 500 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Descricao));
        }
    }
}
