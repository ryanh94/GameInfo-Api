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
        private readonly IDBFactory _repo;
        private readonly IPasswordService _passwordService;
        public UserService(IDBFactory repo, IPasswordService passwordService)
        {
            _repo = repo;
            _passwordService = passwordService;
        }
        public async Task<bool> CreateUser(UserCreation userCreation)
        {
            using (var con = _repo.GetInstance())
            {
                var exists = await con.Get<User>(x => x.Username.ToLower() == userCreation.Username.ToLower()).FirstOrDefaultAsync();

                if (exists != null)
                {
                    return false;
                }
                var password = _passwordService.GeneratePassword(userCreation.Password);
                try
                {
                    con.Add<User>(new User { Username = userCreation.Username.ToLower(), Password = password });
                    await con.Commit();
                }
                catch (Exception e)
                {

                    return false;
                }
            }
            return true;
        }
    }
}
