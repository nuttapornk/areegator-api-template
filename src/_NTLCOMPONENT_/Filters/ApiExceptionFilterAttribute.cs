using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Exceptions;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    public ApiExceptionFilterAttribute()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },                
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
                { typeof(Exception), UnHandleException },
                { typeof(InternalServerErrorException), UnHandleInternalServerErrorException}
            };
    }
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }
    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        _exceptionHandlers[typeof(Exception)].Invoke(context);
    }


    private void HandleValidationException(ExceptionContext context)
    {
        IDictionary<string, string[]> errors = ((ValidationException)context.Exception).Errors;
        var response = new BaseResponse<IDictionary<string, string[]>>(errors)
        {
            Code = StatusCodes.Status400BadRequest.ToString(),
            DataSource = "Validation",
            Message = context.Exception.Message,
            Error = new Error
            {
                 Code = "VALIDATE001",
                 Message = context.Exception.Message,
            }
        };
        context.Result = new JsonResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };

        context.ExceptionHandled = true;

        //var exception = (ValidationException)context.Exception;
        //context.Result = new BadRequestObjectResult(exception.Message);
        //context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        context.Result = new BadRequestObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var response = new BaseResponse
        {
            Code = StatusCodes.Status404NotFound.ToString(),
            DataSource = "N/A",
            Message = context.Exception.Message,
            Error = new Error
            {
                Code = "NOTFOUND001",
                Message = "Data not found,Please contact admin."
            }
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        context.Result = new UnauthorizedResult();
        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        context.Result = new ForbidResult();
        context.ExceptionHandled = true;
    }

    private void UnHandleException(ExceptionContext context)
    {
        var response = new BaseResponse
        {
            Code = StatusCodes.Status500InternalServerError.ToString(),
            DataSource = "N/A",
            Message = context.Exception.Message,
            Error = new Error
            {
                Code = "UNHANDLE001",
                Message = "An error occurred,Please contact admin."
            }
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };

        context.ExceptionHandled = true;
    }

    private void UnHandleInternalServerErrorException(ExceptionContext context)
    {
        var exception = (InternalServerErrorException)context.Exception;
        var response = new BaseResponse
        {
            Code = StatusCodes.Status500InternalServerError.ToString(),
            DataSource = "N/A",
            Message = context.Exception.Message,
            Error = new Error
            {
                Code = exception.ErrorCode,
                Message = exception.ErrorMessage
            }
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = StatusCodes.Status200OK
        };
        context.ExceptionHandled = true;
    }
}
