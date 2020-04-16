using GameInfo.Core.Interfaces;
using GameInfo.Core.Models.Entities;
using GameInfo.Core.Models.Requests;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repo;
        private readonly IPasswordService _passwordService;
        public UserService(IRepository repo, IPasswordService passwordService)
        {
            _repo = repo;
            _passwordService = passwordService;
        }
        public async Task<bool> CreateUser(UserCreation userCreation)
        {

            var exists = await _repo.Get<User>(x => x.Username.ToLower() == userCreation.Username.ToLower()).FirstOrDefaultAsync();

            if (exists != null)
            {
                return false;
            }
            var password = _passwordService.GeneratePassword(userCreation.Password);

            _repo.Add<User>(new User { Username = userCreation.Username.ToLower(), Password = password });
            await _repo.Commit();


            return true;
        }
    }
}
