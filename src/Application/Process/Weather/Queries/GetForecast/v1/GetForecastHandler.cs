using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.ReqRes;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Weather.Queries.GetForecast.v1;

public class GetForecastHandler : IRequestHandler<GetForecastQuery, IEnumerable<GetForecastResponse>>
{
    private readonly IWeatherRepository _weatherRepository;
    private readonly IMapper _mapper;

    public GetForecastHandler(IWeatherRepository weatherRepository,IMapper mapper)
    {
        _weatherRepository = weatherRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetForecastResponse>> Handle(GetForecastQuery request, CancellationToken cancellationToken)
    {
        var req = _mapper.Map<GetWeatherForecastRequest>(request);
        var weatherForecasts = await _weatherRepository.GetForecastAsync(req,cancellationToken);

        var result = _mapper.Map<IEnumerable<GetForecastResponse>>(weatherForecasts);
        return result;


    }
}
