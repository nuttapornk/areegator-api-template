using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Exceptions;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.ExternalApi;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Demo1.Queries.InquiryBanner.v1;

public class InquiryBannerHandler : IRequestHandler<InquiryBannerQuery, IEnumerable<InquiryBannerResponse>>
{
    private readonly ISubbrokerProcessApi _subbrokerProcessApi;
    public InquiryBannerHandler(ISubbrokerProcessApi subbrokerProcessApi)
    {
        _subbrokerProcessApi = subbrokerProcessApi;
    }

    public async Task<IEnumerable<InquiryBannerResponse>> Handle(InquiryBannerQuery request, CancellationToken cancellationToken)
    {
        var data = await _subbrokerProcessApi.InquiryBanner(new Common.models.ExternalApi.SubbrokerProcess.Requests.InquiryBannerRequest());
        if (data.Code == StatusCodes.Status200OK.ToString())
        {
            return data.Data?.Banners?.Select(a => new InquiryBannerResponse
            {
                Item = a.Item,
                Image = a.Image,
                ImageMobile = a.ImgMobile,
                Link = a.Link
            }).ToList();
        }
        else
        {
            throw new InternalServerErrorException("this is exception", errorCode: "code1234", errorMessage: "message 1234");
        }
    }
}
