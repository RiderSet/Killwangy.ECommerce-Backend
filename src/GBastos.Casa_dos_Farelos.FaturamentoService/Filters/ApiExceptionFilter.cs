using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GBastos.Casa_dos_Farelos.FaturamentoService.Api.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(new
        {
            error = context.Exception.Message
        })
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}