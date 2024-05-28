using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetCountries.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ApiControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        public WeatherController(ILogger<WeatherController> logger)
        {

            _logger = logger;
        }


        [HttpPost]
        [Route("v1/GetCountries")]
        public async Task<IActionResult> GetCountry()
        {
            var result = await Mediator.Send(new GetCountriesQuery());
            return Ok(BaseResponse.Ok(result));
        }

        [HttpPost]
        [Route("v1/Forecast")]
        public async Task<IActionResult> GetForecast(GetForecastQuery request)
        {
            var result = await Mediator.Send(request);
            return Ok(BaseResponse.Ok(result));
        }
    }
}
