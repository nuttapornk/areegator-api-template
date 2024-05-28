using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;

public interface IWeatherRepository
{
    Task<IEnumerable<Country>> GetCountriesAsync(CancellationToken cancellationToken);

    Task<int> GetForecastTotalAsync(CancellationToken cancellationToken,GetWeatherForecastRequest request);

    Task<IEnumerable<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken, GetWeatherForecastRequest request);
}
