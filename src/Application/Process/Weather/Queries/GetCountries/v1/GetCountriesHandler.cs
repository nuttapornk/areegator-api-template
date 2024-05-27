using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetCountries.v1;

public class GetCountriesHandler : IRequestHandler<GetCountriesQuery,IEnumerable<GetCountriesResponse>>
{
    private readonly IWeatherRepository _weatherRepository;
    public GetCountriesHandler(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    public async Task<IEnumerable<GetCountriesResponse>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _weatherRepository.GetCountriesAsync(cancellationToken);
        return countries.Select(a => new GetCountriesResponse
        {
            Id = a.Id,
            Name = a.Name,
        }).ToList();
    }
}
