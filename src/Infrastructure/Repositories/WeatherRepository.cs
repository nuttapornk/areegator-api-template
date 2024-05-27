using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IAppDbContext _appDbContext;
        public WeatherRepository(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Country>> GetCountriesAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.Countries.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken, Guid countryId)
        {
            return await _appDbContext.WeatherForecasts
                .Where(a=>a.CountryId == countryId)
                .ToListAsync(cancellationToken);
        }
    }
}
