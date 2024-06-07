using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models.ExternalApi.SubbrokerProcess.Requests;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.models.ExternalApi.SubbrokerProcess.Responses;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using Refit;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.ExternalApi;

public interface ISubbrokerProcessApi
{
    [Get("/subbrokerprocess/v1/alive")]
    Task<AliveResponse> Alive();

    [Post("/subbrokerprocess/v1/Home/v1/inquirybanner")]
    Task<BaseResponse<InquiryBannerResponse>> InquiryBanner([Body] InquiryBannerRequest request);
}
