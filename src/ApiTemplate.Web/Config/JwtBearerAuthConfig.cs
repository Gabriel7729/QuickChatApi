using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ApiTemplate.Web.Config;

public static class JwtBearerAuthConfig
{
  public static IServiceCollection ConfigJwtAuth(this IServiceCollection services, IConfiguration configuration)
  {
    var key = Encoding.ASCII.GetBytes(configuration["JWT_SECRET"]);

    services
      .AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(o =>
      {
        o.TokenValidationParameters = new TokenValidationParameters
        {
          RequireExpirationTime = true,
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = configuration["JWT_ISSUER"],
          ValidAudience = configuration["JWT_AUDIENCE"],
          IssuerSigningKey = new SymmetricSecurityKey(key)
        };
      });
    return services;
  }
}
