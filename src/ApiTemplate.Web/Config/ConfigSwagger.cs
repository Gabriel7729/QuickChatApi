using Microsoft.OpenApi.Models;

namespace ApiTemplate.Web.Config;

public static class ConfigSwagger
{
  public static IServiceCollection SwaggerConfig(this IServiceCollection services)
  {
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Version = "v1",
        Title = "Clean Template Api",
        Description = "A simple example ASP.NET Core Web API",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
          Name = "Gabriel Ernesto De La Rosa Paniagua",
          Email = "gabrieldelarosa2928@gmail.com",
          Url = new Uri("https://www.linkedin.com/in/karlyn-gabriel-garcia-rojas-4446b0246/"),
        },
        License = new OpenApiLicense
        {
          Name = "Solvex License",
          Url = new Uri("https://example.com/license"),
        }
      });
      c.EnableAnnotations();

      c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and your token in the text input below. Example: 'Bearer 12345abcdef'"
      });
      c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
    });
    return services;
  }
}
