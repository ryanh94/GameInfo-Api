using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameInfo.Core.Interfaces;
using GameInfo.Core.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameInfo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create([FromBody]UserCreation user)
        {
            var response = await _userService.CreateUser(user);
            if (response)
            {
                return Ok(response);
            }
            return StatusCode(500);
        }
    }
}