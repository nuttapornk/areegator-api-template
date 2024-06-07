using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;

public class MessageLogger
{
    [JsonProperty("index")]
    public string Index { get; set; }

    [JsonProperty("trace_id")]
    public string TraceId { get; set; }

    [JsonProperty("request_url")]
    public string RequestUrl { get; set; }

    [JsonProperty("request_uri")]
    public string RequestUri { get; set; }

    [JsonProperty("requested_date")]
    public string RequestedDate { get; set; }

    [JsonProperty("response_date")]
    public string ResponseDate { get; set; }

    [JsonProperty("request")]
    public MessageLoggerInfo Request { get; set; }

    [JsonProperty("response")]
    public MessageLoggerInfo Response { get; set; }

    [JsonProperty("curl")]
    public string Curl { get; set; }
}
