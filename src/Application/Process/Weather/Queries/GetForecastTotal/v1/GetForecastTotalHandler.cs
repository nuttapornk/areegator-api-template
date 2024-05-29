using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecastTotal.v1;

public class GetForecastTotalHandler : IRequestHandler<GetForecastTotalQuery, int>
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly IMapper _mapper;

    public GetForecastTotalHandler(IWeatherRepository weatherRepository, IMapper mapper)
    {
        _weatherRepository = weatherRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(GetForecastTotalQuery request, CancellationToken cancellationToken)
    {
        var req = _mapper.Map<GetWeatherForecastRequest>(request);
        return await _weatherRepository.GetForecastTotalAsync(cancellationToken,req);
    }
}
