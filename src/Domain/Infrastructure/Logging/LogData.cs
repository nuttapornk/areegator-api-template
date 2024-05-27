using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;

public class LogData
{
    [JsonProperty("trace_id")]
    public string TraceId { get; set; }

    [JsonProperty("message_type")]
    public string MessageType { get; set; }

    [JsonProperty("message_code")]
    public int MessageCode { get; set; }

    [JsonProperty("message_logs")]
    public string MessageLogs { get; set; }

    [JsonProperty("request_url")]
    public string RequestUrl { get; set; }

    [JsonProperty("requested_date")]
    public string RequestedDate { get; set; }

    [JsonProperty("response_date")]
    public string ResponseDate { get; set; }

    [JsonProperty("request")]
    public object? Request { get; set; }

    [JsonProperty("response")]
    public object? Response { get; set; }
}
