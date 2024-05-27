using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Filters
{
    public class AddHttpContextItemsActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (resultContext.Result is ObjectResult result)
            {
                if (result.Value is not BaseResponse baseResponse)
                {
                    return;
                }
                context.HttpContext.Items.Add("AgResponseCode", baseResponse.Code);

            }
        }

    }
}

