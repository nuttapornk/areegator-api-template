﻿using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using MediatR;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Process.Admin.Queries.GetAgents.v1;

public class GetAgentsQuery : BaseRequest, IRequest<IEnumerable<GetAgentsResponse>>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public Filter Filter { get; set; } = new Filter();    
}
public class Filter
{
    public string Name { get; set; } = string.Empty;
    public string AgentId { get; set; } = string.Empty;
    public string AgentCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}