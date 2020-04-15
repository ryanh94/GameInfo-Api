using GameInfo.Core.Models.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Core.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateUser(UserCreation userCreation);
    }
}
