using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
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

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken, GetWeatherForecastRequest request)
        {
            var query = GetForecastQuery(request);
            return await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetForecastTotalAsync(CancellationToken cancellationToken, GetWeatherForecastRequest request)
        {
            var query = GetForecastQuery(request);
            return await query.CountAsync(cancellationToken);
        }

        private IQueryable<WeatherForecast> GetForecastQuery(GetWeatherForecastRequest request)
        {
            var query = _appDbContext.WeatherForecasts
                .Include(a=>a.Country)
                .Where(a => a.CountryId == request.CountryId
                && a.Date >= request.DateBegin
                && a.Date <= request.DateEnd)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Summaries))
            {
                query = query.Where(a => a.Summaries == request.Summaries);
            }

            if (request.TemperatureC != null)
            {
                query = query.Where(a => a.TemperatureC == request.TemperatureC);
            }

            return query;
        }
    }
}
