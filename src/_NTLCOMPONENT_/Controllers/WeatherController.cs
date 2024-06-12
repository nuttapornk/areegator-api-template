using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Commands.CreateCountry.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetCountries.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecastTotal.v1;
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
        [Route("v1/CreateCountry")]
        public async Task<IActionResult> CreateCountry(CreateCountryCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(BaseResponse.Ok(result));
        }

        [HttpPost]
        [Route("v1/GetCountries")]
        public async Task<IActionResult> GetCountry()
        {
            var result = await Mediator.Send(new GetCountriesQuery());
            return Ok(BaseResponse.Ok(result));
        }

        [HttpPost]
        [Route("v1/ForecastTotal")]
        public async Task<IActionResult> GetForecastTotal(GetForecastTotalQuery request)
        {
            var result = await Mediator.Send(request);
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
