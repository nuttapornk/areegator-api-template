namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.HealthCheck;

public class HealthCheckReponse
{
    public string Status { get; set; }
    public IEnumerable<DependencyHealthCheckResponse> HealthChecks { get; set; }
    public TimeSpan HealthCheckDuration { get; set; }
}
