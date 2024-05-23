using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetTotalAgents.v1;

public class GetTotalAgentsHandler : IRequestHandler<GetTotalAgentsQuery, int>
{
    private readonly IAgentRepository _agentRepository;
    public GetTotalAgentsHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }
    public async Task<int> Handle(GetTotalAgentsQuery request, CancellationToken cancellationToken)
    {
        return await _agentRepository.GetTotalAgentsAsync(cancellationToken, new Domain.Entities.AgentFilter
        {
            AgentCode = request.Filter.AgentCode,
            AgentId = request.Filter.AgentId,
            Name = request.Filter.Name,
            UserName = request.Filter.UserName,
        });
    }
}
