using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageBrokers.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;

public static class MessageBrokers
{
    public static IHealthChecksBuilder AddKafkaCheck(
        this IHealthChecksBuilder builder,
        string bootstrapServers,
        string topic,
        string name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name,
            new KafkaHealthCheck(bootstrapServers: bootstrapServers, topic: topic),
            failureStatus,
            tags,
            timeout));
    }
}
