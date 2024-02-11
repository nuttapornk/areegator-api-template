namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Helpers.HealthCheck;

public class HealthCheckAliveReponse
{
    public string? Status { get; set; }
    public IEnumerable<HealthCheckAliveDetailResponse>? HealthChecks { get; set; }
    public TimeSpan HealthCheckDuration { get; set; }
}

public class HealthCheckAliveDetailResponse
{
    public string? Status { get; set; }
    public string? Component { get; set; }
    public string? Description { get; set; }
}