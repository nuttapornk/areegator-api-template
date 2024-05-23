using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Repositories;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Repositories;

public class AgentRepository : IAgentRepository
{
    private readonly IAppDbContext _appDbContext;
    public AgentRepository(IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Dispose()
    { 
        GC.SuppressFinalize(this);
    }

    public async Task<Agent> FindByAgentCodeAsync(string agentCode)
    {
        throw new NotImplementedException();
        //return await _appDbContext.Agents.FirstOrDefault(a => a.AgentCode == agentCode);
    }

    public async Task<Agent> FindByAgentIdAsync(string agentId)
    {
        return await _appDbContext.Agents.FirstOrDefaultAsync(a => a.AgentID == agentId);
    }

    public async Task<IEnumerable<Agent>> GetAgentsAsync(CancellationToken cancellationToken,
        AgentFilter filter, int pageIndex = 1, int pageSize = 10)
    {
        var q = GetAgentQuery(filter);
        return await q.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalAgentsAsync(CancellationToken cancellationToken, AgentFilter filter)
    {
        var q = GetAgentQuery(filter);
        return await q.CountAsync(cancellationToken);
    }

    private IQueryable<Agent> GetAgentQuery(AgentFilter filter)
    {
        var q = _appDbContext.Agents.AsNoTracking().AsQueryable();
        if (filter != null)
        {
            if (!string.IsNullOrEmpty(filter.AgentId))
            {
                q = q.Where(a => a.AgentID == filter.AgentId);
            }

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                q = q.Where(a => a.UserName.Contains(filter.UserName));
            }
        }
        return q;
    }

}
