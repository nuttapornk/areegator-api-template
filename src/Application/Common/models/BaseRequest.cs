using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models;

public class BaseRequest
{
    [JsonProperty("empid")]
    public string? Username { get; set; } = string.Empty;
}
