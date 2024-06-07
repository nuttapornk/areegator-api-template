using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.ExternalApi;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Demo1.Queries.InquiryBanner.v1;
using Microsoft.AspNetCore.Mvc;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Controllers;

[Route("/[controller]")]
[ApiController]
public class ExternalController : ApiControllerBase
{
    private readonly ISubbrokerProcessApi _subbrokerProcessApi;
    public ExternalController(ISubbrokerProcessApi subbrokerProcessApi)
    {
        _subbrokerProcessApi = subbrokerProcessApi;
    }

    [HttpGet]
    [Route("v1/alive")]
    public async Task<IActionResult> GetAlive()
    {
        var data = await _subbrokerProcessApi.Alive();
        return Ok(BaseResponse.Ok(data));
    }

    [HttpGet]
    [Route("v1/GetBanner")]
    public async Task<IActionResult> GetBanner()
    {
        var data = await Mediator.Send(new InquiryBannerQuery());
        return Ok(BaseResponse.Ok(data));
    }
}
