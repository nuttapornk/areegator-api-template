using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Interfaces.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Common;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        //services.ConfigRedis(configuration);
        services.AddScoped<IAgentRepository, AgentRepository>();
        return services;
    }

    private static IServiceCollection ConfigRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();
        services.AddTransient<IRedisCacheService, RedisCacheService>();

        RedisConfig config = new()
        {
            EndPoint = Environment.GetEnvironmentVariable("REDIS_ENDPOINT") ?? string.Empty,
            Username = Environment.GetEnvironmentVariable("REDIS_USERNAME") ?? string.Empty,
            Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD") ?? string.Empty,
            ChannelPrefix = Environment.GetEnvironmentVariable("REDIS_PREFIX") ?? string.Empty,
            DefautlDatabase = int.Parse(Environment.GetEnvironmentVariable("REDIS_DEFAULT") ?? "7"),
            ConnectRetry = 5,
            Ssl = bool.Parse(Environment.GetEnvironmentVariable("REDIS_SSL") ?? "false")
        };

#if DEBUG
        configuration.GetSection("Redis").Bind(config);
#endif

        string endpoint = config.EndPoint.Contains(':') ? config.EndPoint.Split(':')[0] : config.EndPoint;
        int port = int.Parse(config.EndPoint.Contains(':') ? config.EndPoint.Split(':')[1] : "6379");

        services.AddStackExchangeRedisCache(c =>
        {
            c.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            {
                EndPoints = { { endpoint, port } },
                User = config.Username,
                Password = config.Password,
                DefaultDatabase = config.DefautlDatabase,
                ChannelPrefix = config.ChannelPrefix,
                ConnectRetry = config.ConnectRetry,
                Ssl = config.Ssl
            };
        });

        return services;
    }
}
