using GameInfo.Core.Interfaces;
using GameInfo.Core.Models;
using GameInfo.Core.Models.Entities;
using GameInfo.Core.Models.Requests;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.Infrastructure.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IRepository _repo;
        private readonly ITokenBuilderService _tokenBuilderService;
        private readonly IPasswordService _passwordService;
        public AuthenticateService(IRepository repo, ITokenBuilderService tokenBuilderService, IPasswordService passwordService)
        {
            _repo = repo;
            _tokenBuilderService = tokenBuilderService;
            _passwordService = passwordService;
        }
        public async Task<ServiceResult<AuthToken>> Authenticate(SignInRequest request)
        {
            var result = new ServiceResult<AuthToken>();
            result.Success = false;

            var user = await _repo.Get<User>(x => x.Username.ToLower() == request.Username.ToLower()).FirstOrDefaultAsync();
            if (user != null)
            {
                var validPass = _passwordService.ValidatePassword(request.Password, user.Password);
                if (validPass)
                {
                    var auth = _tokenBuilderService.GetToken(user.Id);
                    result.Success = true;
                    result.Value = new AuthToken { token = auth.token, expiry = DateTime.Now.AddMinutes(15) };
                }
            }

            return result;
        }
    }
}
