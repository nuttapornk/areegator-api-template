using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models.ExternalApi.SubbrokerProcess.Responses;
public class AliveResponse
{
    [JsonProperty("alive")]
    public bool Alive { get; set; }

    [JsonProperty("releaseDate")]
    public string ReleaseDate { get; set; }

    [JsonProperty("env")]
    public string Env { get; set; }
}
