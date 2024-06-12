using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Commands.CreateCountry.v1;

public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, CreateCountryResponse>
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly IMapper _mapper;
    public CreateCountryHandler(IWeatherRepository weatherRepository,IMapper mapper)
    {
        _weatherRepository = weatherRepository;
        _mapper = mapper;
    }

    public async Task<CreateCountryResponse> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var req = _mapper.Map<CreateCountryRequest>(request);

        var country = await _weatherRepository.CreateAsync(req,cancellationToken);
        var result = _mapper.Map<CreateCountryResponse>(country);
        return result;
    }
}
