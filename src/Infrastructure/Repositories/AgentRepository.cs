using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories;

public class AgentRepository : IAgentRepository
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public Task<Agent> FindByAgentCodeAsync(string agentCode)
    {
        return Task.FromResult(new Agent
        {
            AgentCode = agentCode,
            AgentId = "A000001",
            FirstName = "FirstName",
            LastName = "LastName"
        });
    }

    public Task<Agent> FindByAgentIdAsync(string agentId)
    {
        return Task.FromResult(new Agent
        {
            AgentId = agentId,
            AgentCode = Guid.NewGuid().ToString(),
            FirstName = "FirstName",
            LastName = "LastName"
        });
    }

    public Task<IEnumerable<Agent>> GetAgentsAsync()
    {
        IEnumerable<Agent> agents = [ 
            new() {
                AgentCode = Guid.NewGuid().ToString(),
                AgentId = "A00000001",
                FirstName = "FristName1",
                LastName = "LastName1"
            }, 
            new() {
                AgentCode = Guid.NewGuid().ToString(),
                AgentId = "A00000002",
                FirstName = "FristName2",
                LastName = "LastName2"
            }
        ];
        return Task.FromResult(agents);
    }

    public Task<int> GetTotalAgentsAsync()
    {
        return Task.FromResult(2);
    }
}
