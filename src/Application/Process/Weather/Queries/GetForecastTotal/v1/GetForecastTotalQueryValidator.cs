using FluentValidation;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecastTotal.v1;

public class GetForecastTotalQueryValidator : AbstractValidator<GetForecastTotalQuery>
{
    public GetForecastTotalQueryValidator()
    {
        RuleFor(v=>v.Username)        
            .NotEmpty();

        RuleFor(v=>v.DateBegin)
            .NotEmpty();

        RuleFor(v=>v.DateEnd)
            .NotEmpty();
    }
}
