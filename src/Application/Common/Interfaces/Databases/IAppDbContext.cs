using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Databases;

public interface IAppDbContext 
{
    DbSet<Agent> Agents { get; }
    DbSet<AgentAddress> AgentAddresses { get; }
    Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    void Dispose();
}
