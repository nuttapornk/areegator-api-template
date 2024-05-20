using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;

public interface IAgentRepository : IDisposable
{
    Task<int> GetTotalAgentsAsync();
    Task<IEnumerable<Agent>> GetAgentsAsync();
    Task<Agent> FindByAgentIdAsync(string agentId);
    Task<Agent> FindByAgentCodeAsync(string agentCode);
}
