namespace e_commerce_cs.Middlewares;

public class HttpException(int statusCode, string? message) : Exception(message)
{
  public int StatusCode { get; } = statusCode;
}

