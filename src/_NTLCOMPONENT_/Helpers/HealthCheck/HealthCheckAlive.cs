using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.HealthCheck;

public class HealthCheckAlive
{
    public static async Task WriteAsync(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";
        var response = new HealthCheckAliveReponse
        {
            Status = report.Status.ToString(),
            HealthChecks = report.Entries.Select(x => new HealthCheckAliveDetailResponse
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description ?? string.Empty
            }),
            HealthCheckDuration = report.TotalDuration
        };
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
