using Microsoft.AspNetCore.Http;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.MessageHandlers;

public class ForwardHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly string[] TARGET_HEADERS = ["authorization", "sender", "refer", "forward"];
    public ForwardHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor == null || _httpContextAccessor.HttpContext == null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        var targetHeaders = _httpContextAccessor.HttpContext
            .Request.Headers.Where(h =>
                TARGET_HEADERS.Contains(h.Key, StringComparer.InvariantCultureIgnoreCase));

        foreach (var header in targetHeaders)
        {
            if (!request.Headers.Contains(header.Key))
            {
                request.Headers.Add(header.Key, header.Value.ToString());
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}
