using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.HealthCheck;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;

public class HealthCheckResponseWriter
{
    public static async Task WriteAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckReponse
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new DependencyHealthCheckResponse
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description ?? string.Empty
            }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    public static async Task AliveAsync(HttpContext context, HealthReport report)
    {
        var options = context.RequestServices.GetRequiredService<IOptions<HealthCheckResponseOptions>>().Value;
        context.Response.ContentType = "application/json";
        var response = new
        {
            alive = report.Status == HealthStatus.Healthy,
            releaseDate = options.FileVersion,
            env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty,
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
