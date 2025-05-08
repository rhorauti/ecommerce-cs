using e_commerce_cs.aws;
using e_commerce_cs.Middlewares;
using e_commerce_cs.MongoDB;
using e_commerce_cs.Repositories;
using e_commerce_cs.Services;

namespace e_commerce_cs.Infrastructure
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
      services.AddSingleton<MongoDbService>();
      services.AddScoped<AuthRepository>();
      services.AddScoped<HashService>();
      services.AddScoped<JwtService>();
      services.AddScoped<EmailService>();
      services.AddScoped<S3Connector>();
      services.AddScoped<ProductRepository>();
      return services;
    }
  }
}
