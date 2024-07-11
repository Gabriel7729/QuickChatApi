using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Core.Entities.UserAggregate.Specifications;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Models;
using ApiTemplate.SharedKernel.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiTemplate.Infrastructure.Services;
public class AuthService : IAuthService
{
  private readonly IRepository<User> _userRepository;
  private readonly IConfiguration _config;

  public AuthService(
    IRepository<User> userRepository,
    IConfiguration config)
  {
    _userRepository = userRepository;
    _config = config;
  }

  public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
  {
    var spec = new GetUserByEmailSpec(email);
    var user = await _userRepository.GetBySpecAsync(spec, cancellationToken)
      ?? throw new Exception($"The credentials you entered are invalid");

    if (!ValidPassword(user.Password, password))
      throw new Exception("The credentials you entered are invalid");

    return GenerateToken(user);
  }
  public async Task<string> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
  {
    var tokenDeserialized = DeserializeToken(token);

    if (tokenDeserialized.ValidTo < DateTime.UtcNow)
      throw new AuthenticationException("The token has expired");

    string email = tokenDeserialized.Claims.Where(x => x.Type == "email").FirstOrDefault()!.Value
      ?? throw new Exception($"The email was not found in the token");

    GetUserByEmailSpec spec = new(email);
    User? user = await _userRepository.GetBySpecAsync(spec, cancellationToken)
      ?? throw new Exception($"The user was not found");

    return GenerateToken(user);
  }
  public TokenClaim DecryptToken(string token)
  {
    IEnumerable<Claim> claims = DeserializeToken(token).Claims;

    TokenClaim tokenClaim = new();
    tokenClaim.Token = token;

    foreach (var claim in claims)
    {
      switch (claim.Type)
      {
        case "userId":
          tokenClaim.UserId = Guid.Parse(claim.Value);
          break;
        case "name":
          tokenClaim.Name = claim.Value;
          break;
        case "lastName":
          tokenClaim.LastName = claim.Value;
          break;
        case "phoneNumber":
          tokenClaim.PhoneNumber = claim.Value;
          break;
        case "email":
          tokenClaim.Email = claim.Value;
          break;
        case "isEmaiValidated":
          tokenClaim.IsEmailValidated = Convert.ToBoolean(claim.Value);
          break;
        default:
          break;
      }
    }

    return tokenClaim;
  }

  private static JwtSecurityToken DeserializeToken(string token)
  {
    var manejadorJwtToken = new JwtSecurityTokenHandler();

    if (manejadorJwtToken.ReadToken(token) is not JwtSecurityToken tokenDeserializado)
      throw new Exception("The token is not valid");

    return tokenDeserializado;
  }
  private string GenerateToken(User user)
  {
    var secretKey = Encoding.ASCII.GetBytes(_config["JWT_SECRET"]);
    var expiresMinutes = int.Parse(_config["JWT_EXPIRES_MINUTES"]);

    var claims = new ClaimsIdentity(new[] {
                new Claim("userId", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("lastName", user.LastName),
                new Claim("phoneNumber", user.PhoneNumber),
                new Claim("email", user.Email),
                new Claim("isEmaiValidated", user.UserAction!.IsEmailValidated.ToString()),
    });
    var signInCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = claims,
      Expires = DateTime.Now.AddMinutes(expiresMinutes),
      NotBefore = DateTime.Now,
      SigningCredentials = signInCredentials,
      Issuer = _config["JWT_ISSUER"],
      Audience = _config["JWT_AUDIENCE"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
  private static bool ValidPassword(string encryptedPassword, string password)
  {
    return BCrypt.Net.BCrypt.Verify(password, encryptedPassword);
  }
}
