using GameInfo.Core.Interfaces;
using GameInfo.Core.Model;
using GameInfo.Core.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameInfo.Infrastructure.Services
{
    public class TokenBuilderService : ITokenBuilderService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly ITokenProviderService _tokenProviderService;

        public TokenBuilderService(ITokenProviderService tokenProviderService, IOptions<AppSettings> settings)
        {
            _settings = settings;
            _tokenProviderService = tokenProviderService;
        }
        public AuthToken GetToken(int userId)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("UserId", userId.ToString()));

            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.Default.GetBytes(_settings.Value.TokenSettings.Key)),
                    SecurityAlgorithms.HmacSha256Signature),
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_settings.Value.TokenSettings.ExpiryTime),
                audience: _settings.Value.TokenSettings.Audience,
                issuer: _settings.Value.TokenSettings.Issuer);

            var tokenSuccess = _tokenProviderService.WriteToken(jwt);

            return new AuthToken { token = tokenSuccess.token};
        }
    }
}
