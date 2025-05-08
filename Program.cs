using System.Text;

using e_commerce_cs.Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddCors((options) =>
{
  options.AddPolicy("DevCors", (corsBuilder) =>
  {
    corsBuilder.WithOrigins(["http://localhost:5173", "http://localhost:5174"])
          .AllowAnyMethod()
          .AllowCredentials()
          .AllowAnyHeader();
  });
  options.AddPolicy("ProdCors", (corsBuilder) =>
  {
    corsBuilder.WithOrigins("https://myProductionSite.com")
          .AllowAnyMethod()
          .AllowCredentials()
          .AllowAnyHeader();
  });
});
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
  throw new InvalidOperationException("A chave secreta do JWT não foi configurada corretamente.");
}
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
  opt.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = false,
    ValidateAudience = true,
    ValidAudience = "rhorauti-ecommerce",
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
  };
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseCors("DevCors");
}
else
{
  app.UseCors("ProdCors");
}

app.UseMiddleware<e_commerce_cs.Middlewares.ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
