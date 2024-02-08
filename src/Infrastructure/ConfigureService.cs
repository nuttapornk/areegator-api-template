using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure
{
    public static class ConfigureService
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration){
            return services;
        }
    }
}