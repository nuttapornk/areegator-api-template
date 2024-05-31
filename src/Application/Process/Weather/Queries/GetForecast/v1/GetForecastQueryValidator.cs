using FluentValidation;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;

public class GetForecastQueryValidator : AbstractValidator<GetForecastQuery>
{
    public GetForecastQueryValidator()
    {
        RuleFor(v => v.Username)
            .NotEmpty();

        RuleFor(v => v.DateBegin)
            .NotEmpty();

        RuleFor(v => v.DateEnd)
            .NotEmpty();

        RuleFor(v => v.PageIndex)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .NotEmpty()
            .GreaterThan(0);
    }
}
