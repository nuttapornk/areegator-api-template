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

        public async Task<Country> GetCountry(Guid id,CancellationToken cancellationToken)
        {
            return await _appDbContext.Countries.FirstOrDefaultAsync(a => a.Id == id,cancellationToken);
        }

        public async Task<bool> IsExistCountry(string name, CancellationToken cancellationToken)
        {
            return await _appDbContext.Countries.AnyAsync(a => a.Name.ToLower() == name.ToLower());
        }

        public async Task<Country> CreateAsync(CreateCountryRequest request ,CancellationToken cancellationToken)
        {
            Country country = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };
            _appDbContext.Countries.Add(country);
            await _appDbContext.SaveChangeAsync(cancellationToken);
            return country; 
        }

        public async Task UpdateAsync(Country country,CancellationToken cancellationToken)
        {
            _appDbContext.Countries.Update(country);
            await _appDbContext.SaveChangeAsync(cancellationToken);
        }

        public async Task DeleteAsync(Country country,CancellationToken cancellationToken)
        {
            _appDbContext.Countries.Remove(country);
            await _appDbContext.SaveChangeAsync(cancellationToken);
        }

        public async Task<IEnumerable<Country>> GetCountriesAsync(CancellationToken cancellationToken)
        {
            return await _appDbContext.Countries.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(GetWeatherForecastRequest request, CancellationToken cancellationToken)
        {
            var query = GetForecastQuery(request);
            return await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetForecastTotalAsync(GetWeatherForecastRequest request,CancellationToken cancellationToken)
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
