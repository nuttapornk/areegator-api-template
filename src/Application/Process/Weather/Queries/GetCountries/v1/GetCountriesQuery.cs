using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetCountries.v1;

public class GetCountriesQuery : BaseRequest, IRequest<IEnumerable<GetCountriesResponse>>
{

}
