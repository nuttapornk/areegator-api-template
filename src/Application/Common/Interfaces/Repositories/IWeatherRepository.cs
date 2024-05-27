using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;

public interface IWeatherRepository
{
    Task<IEnumerable<Country>> GetCountriesAsync(CancellationToken cancellationToken);

    Task<IEnumerable<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken,Guid countryId);
}
