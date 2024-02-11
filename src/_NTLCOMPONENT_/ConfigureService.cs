using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.NSwag;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;

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

        //Middleware     
        services.AddTransient<RequestHeaderMiddleware>();


        return services;
    }
}
