using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Domain.Entities;
using AutoMapper;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Mappings;

public class AgentAutoMapper: Profile
{
    public AgentAutoMapper()
    {
        CreateMap<Agent, GetAgentsResponse>();
    }
}
