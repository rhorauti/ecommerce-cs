namespace e_commerce_cs.Models
{
  public class ApiResponse<T>
  {
    public bool? Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public string? Token { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Ok(string message, T? data = default, string token = "")
    {
      return new ApiResponse<T>
      {
        Status = true,
        Message = message,
        Token = token,
        Data = data,
      };
    }

    public static ApiResponse<T> Error(string message, List<string>? errors = null)
    {
      return new ApiResponse<T>
      {
        Status = false,
        Message = message,
        Errors = errors ?? []
      };
    }
  }
}
