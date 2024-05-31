using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.MessageBrokers;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;

public class ConfluentKafkaLogging : IConfluentKafkaLogging
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private const string KafkaIndexConfigKey = "KafkaOptions:Index";

    private readonly string _elasticIndexName;
    private readonly ILogger _logger;

    private readonly IKafkaProducer<ApplicationLog> _kafkaApplicationLogProduce;
    private readonly IKafkaProducer<MessageLogger> _kafkaMessageLoggerProduce;

    public ConfluentKafkaLogging(IConfiguration configuration,
        ILogger<ConfluentKafkaLogging> logger,
        IKafkaProducer<ApplicationLog> kafkaApplicationLogProduce,
        IKafkaProducer<MessageLogger> kafkaMessageLoggerProduce)
    {
        _logger = logger;
        _kafkaApplicationLogProduce = kafkaApplicationLogProduce;
        _kafkaMessageLoggerProduce = kafkaMessageLoggerProduce;

        // TODO Refactoing
        //
        var kafkaIndex = configuration[KafkaIndexConfigKey] ?? "";
        var indexFormat = string.Empty;
        GetKafkaIndex(ref kafkaIndex, ref indexFormat);
        //

        _elasticIndexName = !string.IsNullOrEmpty(indexFormat) ? $"{kafkaIndex}-{DateTime.Now.ToString(indexFormat)}" : kafkaIndex;
    }

    public void Error(int? statusCode, string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "ERROR";
            if (request != null)
            {
                messageType = LogType.RequestError;
            }

            if (response != null)
            {
                messageType = LogType.ResponseError;
            }

            ApplicationLog kafkaMessage = new()
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = StatusCodes.Status500InternalServerError,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };

            Task.Run(() => _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void Info(string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "INFO";

            if (request != null)
            {
                messageType = LogType.RequestInfo;
            }

            if (response != null)
            {
                messageType = LogType.ResponseInfo;
            }

            ApplicationLog kafkaMessage = new()
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = StatusCodes.Status200OK,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };

            Task.Run(() => _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void Warning(int? statusCode, string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "WARNING";

            if (request != null)
            {
                messageType = LogType.RequestWarning;
            }

            if (response != null)
            {
                messageType = LogType.ResponseWarning;
            }

            ApplicationLog kafkaMessage = new()
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = statusCode ?? StatusCodes.Status200OK,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };


            Task.Run(() => _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public void Logger(MessageLogger message, string traceId)
    {
        try
        {
            Task.Run(() => _kafkaMessageLoggerProduce.SendAsync(traceId, message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task ErrorAsync(int? statusCode, string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "ERROR";

            if (request != null)
            {
                messageType = LogType.RequestError;
            }

            if (response != null)
            {
                messageType = LogType.ResponseError;
            }

            ApplicationLog kafkaMessage = new()
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = statusCode ?? StatusCodes.Status500InternalServerError,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };
            await _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        await Task.CompletedTask;
    }

    public async Task InfoAsync(string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "INFO";

            if (request != null)
            {
                messageType = LogType.RequestInfo;
            }

            if (response != null)
            {
                messageType = LogType.ResponseInfo;
            }

            var kafkaMessage = new ApplicationLog
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = StatusCodes.Status200OK,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };
            await _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        await Task.CompletedTask;
    }

    public async Task WarningAsync(int? statusCode, string path, object? request, object? response, string traceId)
    {
        try
        {
            var messageType = "WARNING";

            if (request != null)
            {
                messageType = LogType.RequestWarning;
            }

            if (response != null)
            {
                messageType = LogType.ResponseWarning;
            }

            var kafkaMessage = new ApplicationLog
            {
                Index = _elasticIndexName,
                Data = new LogData
                {
                    MessageCode = statusCode ?? StatusCodes.Status200OK,
                    MessageType = messageType,
                    Request = request,
                    RequestUrl = path,
                    Response = response,
                    RequestedDate = DateTime.Now.ToString(DateTimeFormat),
                    ResponseDate = DateTime.Now.ToString(DateTimeFormat),
                    TraceId = traceId
                }
            };
            await _kafkaApplicationLogProduce.SendAsync(traceId, kafkaMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        await Task.CompletedTask;
    }

    public async Task LoggerAsync(MessageLogger message, string traceId)
    {
        try
        {
            await _kafkaMessageLoggerProduce.SendAsync(traceId, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
        await Task.CompletedTask;
    }

    private void GetKafkaIndex(ref string index, ref string format)
    {
        try
        {
            var match = Regex.Match(index, @"{.*?}");
            var trimChars = new[] { '-', '{', '}' };
            var separator = new[] { ':' };

            if (match.Success)
            {
                format = match.Value.Trim(trimChars).Split(separator)[1];
                index = index.Replace(match.Value, string.Empty);
            }
            index = index.TrimStart(trimChars).TrimEnd(trimChars);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

    }
}
