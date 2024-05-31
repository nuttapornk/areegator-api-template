using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Filters;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.NSwag;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares.Logging;
using Newtonsoft.Json.Serialization;


namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_;

public static class ConfigureService
{
    public static IServiceCollection AddComponentService(this IServiceCollection services,
    IConfiguration configuration, string envName)
    {
        services.AddHttpContextAccessor();

        services.AddHealthChecks();

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
        });

        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

        //Middleware     
        services.AddTransient<LoggingMiddleware>();
        services.AddTransient<RequestHeaderMiddleware>();

        return services;
    }
}
