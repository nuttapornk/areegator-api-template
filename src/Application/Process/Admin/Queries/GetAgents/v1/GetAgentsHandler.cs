using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;

public class GetAgentsHandler : IRequestHandler<GetAgentsQuery, IEnumerable<GetAgentsResponse>>
{
    private readonly IAgentRepository _agentRepository;
    private readonly IMapper _mapper;
    
    public GetAgentsHandler(IAgentRepository agentRepository,IMapper mapper)
    {
        _agentRepository = agentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAgentsResponse>> Handle(GetAgentsQuery request, CancellationToken cancellationToken)
    {
        var agent =  await _agentRepository.GetAgentsAsync();
        var result = _mapper.Map<IEnumerable<GetAgentsResponse>>(agent);
        return result;

    }

}
