using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;
using AutoMapper;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;

public class GetForecastResponse : IMapFrom<Domain.Entities.WeatherForecast>
{
    public string Country { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF
    {
        get { return (this.TemperatureC * 9 / 5) + 32; }
    }

    public string? Summaries { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.WeatherForecast, GetForecastResponse>()
            .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Country.Name));            
    }
}
