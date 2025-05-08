using e_commerce_cs.Models;

namespace e_commerce_cs.Middlewares
{
  public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
  {
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;
    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (HttpException ex)
      {
        context.Response.StatusCode = ex.StatusCode;
        context.Response.ContentType = "application/json";
        var apiResponse = ApiResponse<object>.Error(ex.Message);
        await context.Response.WriteAsJsonAsync(apiResponse);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Erro inesperado");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var apiResponse = ApiResponse<object>.Error("Erro interno do servidor");
        await context.Response.WriteAsJsonAsync(apiResponse);
      }
    }
  }
}
