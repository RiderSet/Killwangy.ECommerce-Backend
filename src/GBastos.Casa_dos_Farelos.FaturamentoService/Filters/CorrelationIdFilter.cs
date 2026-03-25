using Microsoft.AspNetCore.Mvc.Filters;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Api.Filters;

public class CorrelationIdFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var correlationId = Guid.NewGuid().ToString();
        context.HttpContext.Items["CorrelationId"] = correlationId;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}