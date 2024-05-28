using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;

public class GetForecastQuery : BaseRequest, IRequest<IEnumerable<GetForecastResponse>>, IMapFrom<GetWeatherForecastRequest>
{
    public Guid CountryId { get; set; }
    public DateOnly DateBegin { get; set; }
    public DateOnly DateEnd { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? TemperatureC { get; set; }
    public string?Summaries { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GetForecastQuery, GetWeatherForecastRequest>()
            .ForMember(d => d.TemperatureC, opt => opt.MapFrom(s => s.TemperatureC));
    }
}
