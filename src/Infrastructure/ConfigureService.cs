using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Common;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration,string envName = "Development")
    {
        //Database
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        //Services
        services.AddTransient<IDateTime, DateTimeService>();
        //services.AddTransient<IConfluentKafkaLogging, ConfluentKafkaLogging>();

        //Repository
        services.AddTransient<IWeatherRepository, WeatherRepository>();

        //services.ConfigRedis(configuration);
        services.ConfigLoggingKafka(configuration, envName);
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

    private static IServiceCollection ConfigLoggingKafka(this IServiceCollection services,
        IConfiguration configuration, string environmentName)
    {

        services.AddTransient<IConfluentKafkaLogging, ConfluentKafkaLogging>();

        KafkaOptions option = new();
        configuration.GetSection("KafkaOptions").Bind(option);

#if DEBUG
        switch (environmentName.ToLower())
        {
            case "development":
                option.BootstrapServers = "mandalorian-dev.ntl.co.th:9092";
                break;
            case "uat":
                option.BootstrapServers = "mandalorian-uat.ntl.co.th:9092";
                break;
            case "preproduction":
                option.BootstrapServers = "mandalorian-pre.ntl.co.th:9092";
                break;
        }
#endif

        services.AddMessageBusSender<ApplicationLog>(option);
        services.AddMessageBusSender<MessageLogger>(option);

        return services;
    }
}
