using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageHandlers;

public class RequestHeaderSetupHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITraceId _traceId;

    public RequestHeaderSetupHandler(IHttpContextAccessor httpContextAccessor,ITraceId traceId)
    {
        _httpContextAccessor = httpContextAccessor;
        _traceId = traceId;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add("TraceId", _traceId.Read());

        if (_httpContextAccessor.HttpContext != null)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Debug", out var debug))
            {
                request.Headers.Add("Debug", debug.First().ToString());
            }

            var apiRequest = _httpContextAccessor.HttpContext.Request;

            request.Headers.Add("Referer", $"{apiRequest?.Scheme}://{apiRequest.Host}{apiRequest.Path}");
        }

        return base.SendAsync(request, cancellationToken);
    }
}
