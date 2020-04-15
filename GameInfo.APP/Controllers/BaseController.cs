using GameInfo.APP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInfo.APP.Controllers
{
    public class BaseController : Controller
    {
        public User GetCurrentUser()
        {
            var claims = User.Claims;

            var user = new User
            {
                UserId = claims.Where(c => c.Type.ToLower() == "userid").FirstOrDefault()?.Value
            };

            return user;
        }
    }
}
