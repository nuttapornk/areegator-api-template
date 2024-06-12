using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    //private readonly IMediator _mediator;

    //private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public AppDbContext(DbContextOptions options) : base (options)
    {
        //_mediator = mediator;
        //_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<WeatherForecast> WeatherForecasts => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
