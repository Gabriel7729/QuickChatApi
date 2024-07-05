using ApiTemplate.Core.Models;

namespace ApiTemplate.Core.Interfaces;
public interface IAuthService
{
  Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
  Task<string> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);
  TokenClaim DecryptToken(string token);
}
