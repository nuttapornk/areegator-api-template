using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;
using Microsoft.AspNetCore.Mvc;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ApiControllerBase
    {
        private readonly ILogger<AdminController> _logger;  
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;   
        }

        [HttpPost]
        [Route("v1/GetTotalAgents")]
        public async Task<IActionResult> GetTotalAgents()
        {
            return Ok();
        }

        [HttpPost]
        [Route("v1/GetAgents")]
        public async Task<IActionResult> GetAgents(GetAgentsQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(BaseResponse.Ok(result));
        }
    }
}
