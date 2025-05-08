using System.Security.Claims;

using e_commerce_cs.DTOs;
using e_commerce_cs.Models;
using e_commerce_cs.Repositories;
using e_commerce_cs.Services;

using Microsoft.AspNetCore.Mvc;

namespace e_commerce_cs.Controllers
{
  [ApiController]
  public class AuthController(AuthRepository authRepository, HashService hashService, JwtService jwtService, EmailService emailService) : ControllerBase
  {

    [HttpPost(ENDPOINT.BASE + "user/login")]
    public async Task<ActionResult<string>> AuthUser([FromBody] UserDTO userDTO)
    {
      if (!ModelState.IsValid)
      {
        var errors = ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList();
        return BadRequest(ApiResponse<string>.Error("Erro de validação", errors));
      }
      User user = await authRepository.GetUser(userDTO.Email);
      if (user == null)
      {
        return NotFound(ApiResponse<string>.Error("Email não encontrado."));
      }
      else
      {
        bool isPasswordOk = hashService.VerifyPassword(user, userDTO.Password);
        if (!isPasswordOk)
        {
          return BadRequest(ApiResponse<string>.Error("Senha inválida."));
        }
        else if (!user.EmailConfirmed)
        {
          return BadRequest(ApiResponse<string>.Error("Email não validado ainda. Verifique sua caixa de e-mail."));
        }
        string jwt = jwtService.GenerateToken(user._id!, user.Email);
        return Ok(ApiResponse<User>.Ok("Usuário autenticado com sucesso!", user, jwt));
      }
    }

    [HttpPost(ENDPOINT.BASE + "user/signup")]
    public async Task<IActionResult> SignUpUser([FromBody] UserDTO userDTO)
    {
      if (!ModelState.IsValid)
      {
        var errors = ModelState.Values
          .SelectMany(v => v.Errors)
          .Select(e => e.ErrorMessage)
          .ToList();
        return BadRequest(ApiResponse<string>.Error("Erro de validação", errors));
      }
      try
      {
        User user = await authRepository.GetUser(userDTO.Email);
        if (user != null)
        {
          return BadRequest(ApiResponse<string>.Error("Email já cadastrado."));
        }
        else
        {
          User userModel = new()
          {
            Name = userDTO.Name,
            Password = userDTO.Password,
            Email = userDTO.Email,
            Avatar = userDTO.Avatar,
            EmailConfirmed = false,
          };
          string hashedPassword = hashService.HashPassword(userModel, userModel.Password);
          userModel.Password = hashedPassword;
          User savedUser = await authRepository.SaveUser(userModel);
          if (savedUser != null)
          {
            string token = jwtService.GenerateToken(savedUser._id!, savedUser.Email);
            _ = emailService.SendEmailConfirmationSignUp(userDTO.Email, userDTO.Name, token);
            return Ok(ApiResponse<User>.Ok("Usuário criado com sucesso.", savedUser));
          }
          return StatusCode(500, ApiResponse<string>.Error("Erro ao salvar o usuário, tente novamente mais tarde."));
        }
      }
      catch (Exception e)
      {
        return BadRequest(ApiResponse<string>.Error("Error: " + e.Message));
      }
    }

    [HttpGet(ENDPOINT.BASE + "user/check-token")]
    public async Task<IActionResult> CheckToken([FromQuery] string token)
    {
      ClaimsPrincipal principal = jwtService.ValidateToken(token)!;
      if (principal == null)
      {
        return BadRequest(ApiResponse<string>.Error("Token inválido."));
      }
      string email = principal?.FindFirst(ClaimTypes.Email)?.Value!;
      User user = await authRepository.GetUser(email);
      if (user == null)
      {
        return NotFound(ApiResponse<string>.Error("Email não encontrado."));
      }
      else
      {
        User updatedUser = await authRepository.UpdateEmailConfirmedAsync(user);
        return Ok(ApiResponse<User>.Ok("Email validado com sucesso", updatedUser));
      }
    }

    [HttpPost(ENDPOINT.BASE + "user/password-recover")]
    public async Task<IActionResult> SendEmailPasswordRecover([FromBody] EmailDTO email)
    {
      User user = await authRepository.GetUser(email.Email);
      if (user == null)
      {
        return BadRequest(ApiResponse<string>.Error("Email não encontrado."));
      }
      string token = jwtService.GenerateToken(user._id!, user.Email);
      _ = emailService.SendEmailConfirmationResetPassword(user.Email, user.Name, token);
      return Ok(ApiResponse<string>.Ok("Email de recuperação de senha enviado com sucesso."));
    }

    [HttpPut(ENDPOINT.BASE + "user/new-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] PasswordDTO newPassword, [FromQuery] string token)
    {
      ClaimsPrincipal principal = jwtService.ValidateToken(token)!;
      if (principal == null)
      {
        return BadRequest(ApiResponse<string>.Error("Token inválido."));
      }
      else
      {
        string email = principal?.FindFirst(ClaimTypes.Email)?.Value!;
        User user = await authRepository.GetUser(email);
        user.Password = hashService.HashPassword(user, newPassword.Password);
        User updatedUser = await authRepository.UpdatePasswordAsync(user, user.Password);
        return Ok(ApiResponse<User>.Ok("Senha atualizada com sucesso.", updatedUser));
      }
    }
  }
}
