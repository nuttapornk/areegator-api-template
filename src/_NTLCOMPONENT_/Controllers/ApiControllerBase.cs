using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator = null;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
