using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.ExternalApi;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Common;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Caching;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageHandlers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Net;

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
        services.AddSingleton<ITraceId, TraceIdService>();
        //services.AddTransient<IConfluentKafkaLogging, ConfluentKafkaLogging>();

        //Repository
        services.AddTransient<IWeatherRepository, WeatherRepository>();

        //services.ConfigRedis(configuration);
        services.ConfigLoggingKafka(configuration, envName);

        //Config ExternalApi 
        services.ConfigSubbrokerApi(configuration);

        //MessageHandlers
        services.AddTransient<RequestHeaderSetupHandler>();
        services.AddTransient<ForwardHeaderHandler>();
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

        if (IsDebug())
        {
            configuration.GetSection("Redis").Bind(config);
        }

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

        if (!IsDebug())
        {
            //kube create environment variable
            option.BootstrapServers = Environment.GetEnvironmentVariable("KafkaOptions_BootstrapServers") ?? string.Empty;  
        }

        services.AddMessageBusSender<ApplicationLog>(option);
        services.AddMessageBusSender<MessageLogger>(option);

        return services;
    }

    private static IServiceCollection ConfigSubbrokerApi(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("ExternalService").GetChildren()
            .FirstOrDefault(s => s.GetSection("SubbrokerProcessApi") != null)?.GetSection("SubbrokerProcessApi")
            .Get<ExternalApiConfig>() ?? throw new NotImplementedException("Please imprement ExternalService:SubbrokerProcessApi appsettings.[env].json");

        services.AddRefitClient<ISubbrokerProcessApi>(new RefitSettings
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer()
        })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                if (config.ProxyEnable)
                {
                    var proxy = new WebProxy
                    {
                        Address = new Uri(config.BaseUrl),
                        BypassProxyOnLocal = false,
                        UseDefaultCredentials = config.UseDefaultCredentials
                    };

                    if (!proxy.UseDefaultCredentials)
                    {
                        proxy.Credentials = new NetworkCredential(userName: config.ProxyUser, password: config.ProxyPass);
                    }

                    handler.Proxy = proxy;
                    if (IsDebug())
                    {
                        handler.Proxy = GetLocalProxy();
                    }
                    handler.PreAuthenticate = true;
                    handler.UseDefaultCredentials = false;
                }
                return handler;
            }).ConfigureHttpClient(cfg =>
            {
                cfg.BaseAddress = new Uri(config.BaseUrl);
                cfg.Timeout = TimeSpan.FromSeconds(config.RequestTimeout);
            })
            .AddHttpMessageHandler<RequestHeaderSetupHandler>()
            .AddHttpMessageHandler<ForwardHeaderHandler>();

        return services;
    }

    private static IWebProxy GetLocalProxy()
    {
        return new WebProxy
        {
            Address = new Uri("http://HTTP-Proxy03.cfg.co.th:8080"),
            Credentials = new NetworkCredential("NTL-SVA-API-INTERNET", "Nas3jk!AtqzxEUy7")
        };
    }

    private static bool IsDebug()
    {
#if DEBUG
        return true;
#endif
        return false;
    }
}
