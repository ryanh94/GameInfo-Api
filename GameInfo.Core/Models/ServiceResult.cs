using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models
{
    public class ServiceResult <T>
    {
        public T Value { get; set; }
        public bool Success { get; set; }
    }
}
