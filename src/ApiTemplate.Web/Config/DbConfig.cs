namespace ApiTemplate.Web.Config;

using ApiTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public static class DbConfig
{
  public static IServiceCollection ConfigDbConnection(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration["DATABASE_CONNECTION_STRING"]));
    return services;
  }
}
