using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Demo1.Queries.InquiryBanner.v1;

public class InquiryBannerQuery : BaseRequest,IRequest<IEnumerable<InquiryBannerResponse>>
{

}
