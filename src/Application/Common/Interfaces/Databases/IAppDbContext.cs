using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;

public interface IAppDbContext 
{
    DbSet<Country> Countries { get; }
    DbSet<WeatherForecast> WeatherForecasts { get; }
    Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    void Dispose();
}
