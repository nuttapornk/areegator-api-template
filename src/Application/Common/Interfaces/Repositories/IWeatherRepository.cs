using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;

public interface IWeatherRepository
{
    Task<Country> GetCountry(Guid id, CancellationToken cancellationToken);
    Task<bool> IsExistCountry(string name,CancellationToken cancellationToken);
    Task<Country> CreateAsync(CreateCountryRequest request, CancellationToken cancellationToken);
    Task UpdateAsync(Country country, CancellationToken cancellationToken);
    Task DeleteAsync(Country country, CancellationToken cancellationToken);
    Task<IEnumerable<Country>> GetCountriesAsync(CancellationToken cancellationToken);
    Task<int> GetForecastTotalAsync(GetWeatherForecastRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<WeatherForecast>> GetForecastAsync( GetWeatherForecastRequest request, CancellationToken cancellationToken);
}
