﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
