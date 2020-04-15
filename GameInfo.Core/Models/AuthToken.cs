using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models
{
    public class AuthToken
    {
        public string token { get; set; }
        public DateTime expiry { get; set; }
    }
}
