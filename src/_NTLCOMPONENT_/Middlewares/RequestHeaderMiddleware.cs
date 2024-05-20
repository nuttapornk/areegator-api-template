using System.Net;
using System.Text;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
public class RequestHeaderMiddleware : IMiddleware
{
    private readonly List<string> _whiteLists = [];

    public RequestHeaderMiddleware()
    {
        _whiteLists = ["/alive", "/health", "/swagger"];
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string message = string.Empty;
        try
        {

            foreach (var whiteList in _whiteLists)
            {
                if (context.Request.Path.StartsWithSegments(whiteList))
                {
                    await next(context);
                    return;
                }
            }


            if (!context.Request.Headers.TryGetValue("refer", out Microsoft.Extensions.Primitives.StringValues refer))
            {
                message = "Missing request header - refer";
            }
            else if (!context.Request.Headers.TryGetValue("sender", out Microsoft.Extensions.Primitives.StringValues sender))
            {
                message = "Missing request header - sender";
            }
            else if (!context.Request.Headers.TryGetValue("forward", out Microsoft.Extensions.Primitives.StringValues forward))
            {
                message = "Missing request header - forward";
            }
            else
            {
                if (String.IsNullOrEmpty(refer))
                {
                    message = "Missing value request header - refer";
                }
                else if (String.IsNullOrEmpty(sender))
                {
                    message = "Missing value request header - sender";
                }
                else if (String.IsNullOrEmpty(forward))
                {
                    message = "Missing value request header - forward";
                }
            }

        }
        catch (Exception ex)
        {
            message = ex.Message;
        }

        if (!String.IsNullOrEmpty(message))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            BaseResponse? response = BaseResponse.Error500(devErrorMessage: message);
            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(response), Encoding.UTF8);

            return;
        }

        await next.Invoke(context);
    }
}