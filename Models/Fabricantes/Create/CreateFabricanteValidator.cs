using FluentValidation;

namespace GestaoConcessionariasWebApp.Models.Fabricantes.Create;

public class CreateFabricanteValidator : AbstractValidator<CreateFabricanteDto>
{
    public CreateFabricanteValidator()
    {
        RuleFor(x => x.NomeFabricante)
            .NotEmpty()
            .WithMessage("O nome do fabricante é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do fabricante deve ter no máximo 100 caracteres.");

        RuleFor(x => x.PaisOrigem)
            .NotEmpty()
            .WithMessage("O país de origem é obrigatório.")
            .MaximumLength(50)
            .WithMessage("O país de origem deve ter no máximo 50 caracteres.");

        RuleFor(x => x.AnoFundacao)
            .NotEmpty()
            .WithMessage("O ano de fundação é obrigatório.")
            .InclusiveBetween(0, DateTime.UtcNow.Year)
            .WithMessage($"O ano de fundação deve ser menor do que o ano anual: {DateTime.UtcNow.Year}.");

        RuleFor(x => x.Website)
            .NotEmpty()
            .WithMessage("O website é obrigatório.")
            .Must(IsValidUrl)
            .WithMessage("Website inválido. Por favor, insira no começo do link: http:// ou https://")
            .MaximumLength(255)
            .WithMessage("Website deve ter no máximo 255 caracteres.");
    }

    private static bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var u) && (u.Scheme == Uri.UriSchemeHttp || u.Scheme == Uri.UriSchemeHttps);
    }
}
