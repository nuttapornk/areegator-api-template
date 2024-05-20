namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;

public record GetAgentsResponse
{
    public string AgentId { get; set; }
    public string AgentCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public string FullName { get; set; }

}
