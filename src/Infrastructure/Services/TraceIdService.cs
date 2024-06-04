using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Services;

public class TraceIdService : ITraceId
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _traceId;

    public TraceIdService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _traceId = string.Empty;
}

    public string Read()
    {
        if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Items.TryGetValue("TraceId", out var traceId) &&
                traceId != null)
        {
            return traceId.ToString();
        }

        return _traceId;
    }

    public void Write(string traceId)
    {
        _traceId = traceId;
    }
}
