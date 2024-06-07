using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Demo1.Queries.InquiryBanner.v1;

public class InquiryBannerResponse
{
    [JsonProperty("item")]
    public string Item { get; set; }

    [JsonProperty("img")]
    public string Image { get; set; }

    [JsonProperty("img-mobile")]
    public string ImageMobile { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }
}
