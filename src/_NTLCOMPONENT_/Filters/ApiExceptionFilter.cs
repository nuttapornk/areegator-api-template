using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Filters;

public class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var ex = context.Exception;

        LogException(ex);

        context.ExceptionHandled = true;

        var response = new 
        {
            responsecode = "500",
            responsemessage = ex.Message,
            responsedatasource = "N/A",
            responseerror = new 
            {
                errorcode = "COM900001",
                errormessage = "ระบบเกิดข้อผิดพลาด กรุณาติดต่อเจ้าหน้าที่"
            }
        };

        context.HttpContext.Items.Add("AgResponseCode", response.responsecode);
        context.Result = new JsonResult(response);

        return Task.CompletedTask;
    }

    private void LogException(Exception ex)
    {
        Dictionary<string, object> logDic = new Dictionary<string, object>();

        logDic.Add("ErrorType", ex.GetType().Name);
        logDic.Add("Message", ex.Message);
        logDic.Add("StackTrace", ex.StackTrace);

        if (ex.InnerException != null)
        {
            logDic.Add("InnerExceptionMessage", ex.InnerException.Message);
            logDic.Add("InnerExceptionStackTrace", ex.InnerException.StackTrace);
        }

        string msg = string.Join("|", logDic.Keys.Select(e => "{" + e + "}").ToArray());

        object[] values = logDic.Values.ToArray();

        _logger.LogError(msg, values);

    }
}
