using GameInfo.Core.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace GameInfo.Infrastructure.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        public (string token, bool success) WriteToken(JwtSecurityToken securityToken)
        {
            string jwtToken = "";
            bool tokenSuccess = false;
            try
            {
                jwtToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
                tokenSuccess = true;
            }
            catch (Exception ex)
            {
                //log ex;
            }
            return (jwtToken, tokenSuccess);
        }
    }
}
