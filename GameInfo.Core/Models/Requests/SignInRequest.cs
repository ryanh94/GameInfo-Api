using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models.Requests
{
    public class SignInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
