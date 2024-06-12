using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Filters;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.NSwag;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Persistence;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json.Serialization;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_;

public static class ConfigureService
{
    public static IServiceCollection AddComponentService(this IServiceCollection services,
    IConfiguration configuration, string envName)
    {
        services.AddHttpContextAccessor();

        //Add Cors
        services.AddCors(options =>
        {
            options.AddPolicy("CORS", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
        });

        services.AddHealthChecks()
            .AddCheck<StartupHealthCheck>("Startup", tags: new[] { "startup" })
            .AddDbContextCheck<AppDbContext>(tags: new[] { "ready", "live" })            
            .AddKafkaCheck(
                bootstrapServers: MessageBrokersCollectionExtensions.GetKafkaOption(configuration).BootstrapServers,
                topic: "healthcheck",
                name: "Message Broker (Kafka)",
                failureStatus: HealthStatus.Degraded, tags: new[] { "ready", "live" });

        services.AddOpenApiDocument(config =>
        {
            config.Title = "_NTLCOMPONENT_";
            config.Version = "v1";
            config.Description = $"_NTLCOMPONENT_ {Environment.GetEnvironmentVariable(envName)}";
            config.OperationProcessors.Add(new AddRequiredHeaderParameter());
        });

        //Add Filters
        services.AddControllers(options =>
        {
            options.Filters.Add<AddHttpContextItemsActionFilter>();
            options.Filters.Add<ApiExceptionFilterAttribute>();
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        });            

        //Middleware     
        services.AddTransient<LoggingMiddleware>();
        services.AddTransient<RequestHeaderMiddleware>();

        return services;
    }
}
