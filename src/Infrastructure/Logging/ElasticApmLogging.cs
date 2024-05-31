using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;

public static class ElasticApmLogging
{
    private const string ElasticApmServerUrlConfigKey = "ElasticApm:ServerUrl";
    public static Logger CreateSeriLogger(IConfiguration configuration)
    {
        var elasticUrl = configuration[ElasticApmServerUrlConfigKey] ?? string.Empty;
        if (string.IsNullOrEmpty(elasticUrl))
        {
            throw new Exception("APM server is required.");
        }
        var logger = new LoggerConfiguration().Enrich
            .WithElasticApmCorrelationInfo()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUrl))
            {
                CustomFormatter = new EcsTextFormatter()
            })
            .CreateLogger();
        return logger;
    }
}
