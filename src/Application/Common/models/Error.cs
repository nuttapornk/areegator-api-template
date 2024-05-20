using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;

public class Error
{
    [JsonProperty("errorCode")]
    public string? Code { get; set; } = string.Empty;

    [JsonProperty("errorHeader")]
    public string? Header { get; set; } = string.Empty;

    [JsonProperty("errorMessage")]
    public string? Message { get; set; } = string.Empty;
}
