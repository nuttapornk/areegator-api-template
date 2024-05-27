using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;

public interface IConfluentKafkaLogging
{
    void Error(int? statusCode, string path, object? request, object? response, string traceId);

    void Info(string path, object? request, object? response, string traceId);

    void Warning(int? statusCode, string path, object? request, object? response, string traceId);

    void Logger(MessageLogger message, string traceId);

    Task ErrorAsync(int? statusCode, string path, object? request, object? response, string traceId);

    Task InfoAsync(string path, object? request, object? response, string traceId);

    Task WarningAsync(int? statusCode, string path, object? request, object? response, string traceId);

    Task LoggerAsync(MessageLogger message, string traceId);
}
