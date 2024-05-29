using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecastTotal.v1;

public class GetForecastTotalQuery : BaseRequest,IRequest<int>, IMapFrom<GetWeatherForecastRequest>
{
    public Guid CountryId { get; set; }
    public DateOnly DateBegin { get; set; }
    public DateOnly DateEnd { get; set; }
    public int? TemperatureC { get; set; }
    public string? Summaries { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetForecastTotalQuery, GetWeatherForecastRequest>();            
    }
}
