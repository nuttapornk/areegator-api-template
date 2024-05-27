using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using System.Text;
using Elastic.Apm;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares.Logging;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] excludes = new[] { "alive", "health", "swagger", "favicon" };
    private const string LogDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private const string KafkaIndexConfigKey = "KafkaOptions:Index";
    private readonly ILogger _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger;
    }

    public async Task Invoke(HttpContext context,
        IConfluentKafkaLogging confluentKafkaLogService,
        IConfiguration configuration)
    {
        var requestUri = $"{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{requestUri}";
        var containsAny = ContainsAny(requestUrl, excludes);
        var index = configuration[KafkaIndexConfigKey] ?? string.Empty;

        if (containsAny)
        {
            await _next(context);
        }
        else
        {

            var traceId = Elastic.Apm.Agent.Tracer.CurrentTransaction.TraceId;

            context.Request.Headers.Add("trace-id", traceId);

            try
            {
                // first, get the incoming request
                var request = await FormatRequest(context.Request);
                var messageLog = new MessageLogger
                {
                    Index = index,
                    RequestUrl = requestUrl,
                    RequestUri = requestUri,
                    TraceId = traceId,
                    RequestedDate = DateTime.Now.ToString(LogDateTimeFormat),
                    Request = new MessageLoggerInfo
                    {
                        Body = request,
                        Header = context.Request.Headers,
                        MessageType = LogType.RequestInfo
                    }
                };



                // copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;

                // create a new memory stream
                await using var responseBody = new MemoryStream();
                // and use that for the temporary response body
                context.Response.Body = responseBody;

                // add Trace-Id to response headers
                context.Response.Headers.Add("X-Response-Trace-Id", traceId);

                // continue down the middleware pipeline, eventually returning to this class
                await _next(context);

                // format the response from the server
                var response = await FormatResponse(context.Response);
                // var responseHeader = await FormatRequestHeader(context.Response);

                messageLog.ResponseDate = DateTime.Now.ToString(LogDateTimeFormat);
                messageLog.Response = new MessageLoggerInfo
                {
                    Body = response,
                    Header = context.Response.Headers,
                    StatusCode = context.Response.StatusCode == StatusCodes.Status200OK
                        ? StatusCodes.Status200OK
                        : context.Response.StatusCode,
                    MessageType = context.Response.StatusCode == StatusCodes.Status200OK
                        ? LogType.ResponseInfo
                        : LogType.ResponseError
                };

                // fire and forget logging
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await confluentKafkaLogService.LoggerAsync(messageLog, traceId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"TRACE-ID: {traceId}: {ex.Message}");
                    }
                });

                // copy the contents of the new memory stream (which contains the response)
                // to the original stream, which is then returned to the client
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TRACE-ID: {traceId}: {ex.Message}");
            }
        }
    }

    private async Task<string?> FormatRequest(HttpRequest request)
    {
        try
        {
            _ = request.Body;

            // set the reader for the request back at the beginning of its stream
            request.EnableBuffering();

            // create a new byte[] with the same length as the request stream
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            // and then copy the entire request stream into the new buffer
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length)).ConfigureAwait(false);

            // convert the byte[] into a string using UTF8 encoding
            var text = Encoding.UTF8.GetString(buffer);

            // reset the stream position to 0, which is allowed because of EnableBuffering()
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
            // read the response stream from the beginning
            response.Body.Seek(0, SeekOrigin.Begin);

            // and copy it into a string
            var text = await new StreamReader(response.Body).ReadToEndAsync();

            // reset the reader for the response so that the client can read it
            response.Body.Seek(0, SeekOrigin.Begin);

            // return the string for the response, including the status code (e.g. 200, 400, 500)
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

        return subStrings.Any(substring => testString.Contains(substring, StringComparison.OrdinalIgnoreCase));
    }
}
