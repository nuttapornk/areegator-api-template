using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;

public class GetAgentsQuery : IRequest<IEnumerable<GetAgentsResponse>> 
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public GetAgentsQueryFilter Filter { get; set; } = new GetAgentsQueryFilter();

    public class GetAgentsQueryFilter
    {
        public string Name { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
