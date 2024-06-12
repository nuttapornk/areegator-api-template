using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Commands.CreateCountry.v1;

public class CreateCountryCommand : BaseRequest,IRequest<CreateCountryResponse>, IMapFrom<CreateCountryRequest>
{
    public string Name { get; set; }

    public static void Mapping(MappingProfile profile)
    {
        profile.CreateMap<CreateCountryCommand, CreateCountryRequest>();
    }
}
