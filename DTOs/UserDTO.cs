using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace e_commerce_cs.DTOs
{
  public class UserDTO
  {
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";
    [JsonPropertyName("email")]
    [Required(ErrorMessage = "O campo do e-mail não pode estar vazio.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public required string Email { get; set; }
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    // [RegularExpression(@"^(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "A senha deve ter um caracter especial.")]
    public required string Password { get; set; }
    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

  }

  public class EmailDTO
  {
    [Required(ErrorMessage = "O campo do e-mail não pode estar vazio.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    public required string Email { get; set; }
  }

  public class PasswordDTO
  {
    [JsonPropertyName("password")]
    [Required(ErrorMessage = "O campo senha não pode estar vazio.")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public required string Password { get; set; }
  }

}