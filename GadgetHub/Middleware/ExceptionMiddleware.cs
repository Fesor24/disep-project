using GadgetHub.ViewModels;
using System.Text.Json;

namespace GadgetHub.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var errors = _env.IsDevelopment() ? new ErrorViewModel { Message = ex.Message, Description = ex.StackTrace } :
                new ErrorViewModel { Message = ex.Message, Description = string.Empty };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedError = JsonSerializer.Serialize(errors, jsonOptions);

            await context.Response.WriteAsync(serializedError);
        }
    }
}
