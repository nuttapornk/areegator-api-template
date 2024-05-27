using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Infrastructure.Logging;

public class MessageLoggerInfo
{
    [JsonProperty("message_type")]
    public string MessageType { get; set; }

    [JsonProperty("status_code")]
    public int StatusCode { get; set; }

    [JsonProperty("header")]
    public object Header { get; set; }

    [JsonProperty("body")]
    public object Body { get; set; }
}
