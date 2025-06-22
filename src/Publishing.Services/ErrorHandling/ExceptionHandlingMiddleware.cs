using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Publishing.Services;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IErrorHandler _handler;

    public ExceptionHandlingMiddleware(RequestDelegate next, IErrorHandler handler)
    {
        _next = next;
        _handler = handler;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _handler.Handle(ex);
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
        }
    }
}
