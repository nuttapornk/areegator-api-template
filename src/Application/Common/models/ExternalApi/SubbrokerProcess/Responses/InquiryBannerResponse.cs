using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models.ExternalApi.SubbrokerProcess.Responses;

public class InquiryBannerResponse 
{
    [JsonProperty("banner_home")]
    public List<Banner> Banners { get; set; } = new List<Banner>();

    public class Banner
    {
        [JsonProperty("item")]
        public string Item { get; set; }

        [JsonProperty("img")]
        public string Image { get; set; }

        [JsonProperty("img-mobile")]
        public string ImgMobile { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
