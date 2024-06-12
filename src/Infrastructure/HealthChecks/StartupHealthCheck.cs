using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;

public class StartupHealthCheck : IHealthCheck
{
    private volatile bool _isReady;

    public bool StartupCompleted
    {
        get => _isReady;
        set => _isReady = value;
    }
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("The startup task has completed."));
    }
}
