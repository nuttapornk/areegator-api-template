using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;
using AutoMapper;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Commands.CreateCountry.v1;

public class CreateCountryResponse : IMapFrom<Domain.Entities.Country>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public static void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Country, CreateCountryResponse>();
    }
}
