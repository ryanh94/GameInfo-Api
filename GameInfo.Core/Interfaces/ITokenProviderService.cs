using System.IdentityModel.Tokens.Jwt;

namespace GameInfo.Core.Interfaces
{
    public interface ITokenProviderService
    {
        (string token, bool success) WriteToken(JwtSecurityToken securityToken);
    }
}
