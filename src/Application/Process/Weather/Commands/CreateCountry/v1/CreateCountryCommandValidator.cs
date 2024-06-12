using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using FluentValidation;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Commands.CreateCountry.v1;

public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    private readonly IWeatherRepository _weatherRepository;
    public CreateCountryCommandValidator(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
        RuleFor(a => a.Name)
            .NotEmpty()
            .MaximumLength(50)
            .MustAsync(async (name, cancellationToken) => !await _weatherRepository.IsExistCountry(name, cancellationToken))
            .WithMessage(a=>$"{a.Name} already exists.");
    }
}