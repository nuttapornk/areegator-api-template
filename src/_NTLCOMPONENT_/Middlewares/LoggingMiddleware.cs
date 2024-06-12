using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;
using Elastic.Apm;
using System.Text;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    private readonly string[] excludes = ["/alive", "/health", "/swagger", "/favicon"];
    private const string LogDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private const string KafkaIndexConfigKey = "KafkaOptions:Index";
    private readonly ILogger _logger;

    private readonly IConfiguration _configuration;
    private readonly IConfluentKafkaLogging _confluentKafkaLogging;

    public LoggingMiddleware(ILogger<LoggingMiddleware> logger,
        IConfiguration configuration, IConfluentKafkaLogging confluentKafkaLogging)
    {
        _logger = logger;
        _configuration = configuration;
        _confluentKafkaLogging = confluentKafkaLogging;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestUri = $"{context.Request.Path}{context.Request.QueryString}";
        var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{requestUri}";
        var containsAny = ContainsAny(requestUri, excludes);

        var index = _configuration[KafkaIndexConfigKey] ?? string.Empty;

        if (containsAny)
        {
            await next(context);
        }
        else
        {
            var traceId = Agent.Tracer?.CurrentTransaction?.TraceId ?? Guid.NewGuid().ToString();
            context.Request.Headers.Append("trace-id", traceId);
            try
            {
                var request = await FormatRequest(context.Request);
                var messageLog = new MessageLogger
                {
                    Index = index,
                    RequestUrl = requestUrl,
                    RequestUri = requestUri,
                    TraceId = traceId,
                    RequestedDate = DateTime.Now.ToString(LogDateTimeFormat),
                    Curl = GenerateCurl(requestUrl, context),
                    Request = new MessageLoggerInfo
                    {
                        Body = request,
                        Header = context.Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToString()),
                        MessageType = LogType.RequestInfo
                    }
                };

                var originBodyStream = context.Response.Body;
                await using var responseBody = new MemoryStream();

                context.Response.Body = responseBody;
                context.Response.Headers.Append("X-Response-Trace-Id", traceId);
                await next(context);

                var response = await FormatResponse(context.Response);
                messageLog.ResponseDate = DateTime.Now.ToString(LogDateTimeFormat);
                messageLog.Response = new MessageLoggerInfo
                {
                    Body = response,
                    Header = context.Response.Headers.ToDictionary(a => a.Key, a => a.Value.ToString()),
                    StatusCode = context.Response.StatusCode == StatusCodes.Status200OK ? StatusCodes.Status200OK : context.Response.StatusCode,
                    MessageType = context.Response.StatusCode == StatusCodes.Status200OK ? LogType.ResponseInfo : LogType.ResponseError
                };

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _confluentKafkaLogging.LoggerAsync(messageLog, traceId);
                    }
                    catch (Exception ex)
                    {
                        string message = $"TRACE-ID:{traceId} : {ex.Message}";
                        _logger.LogError(ex, message);
                    }
                });

                await responseBody.CopyToAsync(originBodyStream);
            }
            catch (Exception ex)
            {
                string message = $"TRACE-ID:{traceId} : {ex.Message}";
                _logger.LogError(ex, message);
            }
        }
    }

    private async Task<string?> FormatRequest(HttpRequest request)
    {
        try
        {
            _ = request.Body;
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

            var text = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return default;
    }
    private static async Task<string?> FormatRequestHeader(HttpRequest request)
    {
        if (request == null)
        {
            return null;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("--- Request Header ---");
        foreach (var header in request.Headers)
        {
            sb.AppendLine($"{header.Key}: {header.Value}");
        }

        return sb.ToString();

    }

    private async Task<string?> FormatResponse(HttpResponse response)
    {
        try
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return default;
    }

    private static bool ContainsAny(string testString, string[] subStrings)
    {
        if (string.IsNullOrEmpty(testString) || subStrings == null)
            return false;

        return subStrings.Any(substring => testString.StartsWith(substring, StringComparison.OrdinalIgnoreCase));
    }

    private string GenerateCurl(string requestUrl, HttpContext context)
    {
        string[] headersIncludes = ["sender", "forward", "refer", "singnature", "content-type", "referer", "authorization"];
        var curlCommand = new StringBuilder("curl");
        
        // Add the request URL
        curlCommand.Append($" -X {context.Request.Method} \"{requestUrl}\"");

        // Add headers
        foreach (var header in context.Request.Headers)
        {
            if (headersIncludes.Contains(header.Key.ToLower()))
            {
                curlCommand.Append($" -H \"{header.Key}: {header.Value}\"");
            }
        }

        // Add request body if it exists
        var requestBody = FormatRequest(context.Request).Result;
        if (!string.IsNullOrEmpty(requestBody))
        {
            curlCommand.Append($" -d '{requestBody}'");
        }

        return curlCommand.ToString();
    }


}
