using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ExternalApi.SubbrokerProcess.Responses;

public class InquiryBannerResponse
{
    [JsonProperty("banner_home")]
    public List<BannerItem> BannerHome { get; set; } = []; 
    public class BannerItem
    {
        [JsonProperty("link")]
        public int Item { get; set; }

        [JsonProperty("img")]
        public string Img { get; set; }

        [JsonProperty("img-mobile")]
        public string ImgMobile { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
