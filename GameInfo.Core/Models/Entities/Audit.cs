using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models.Entities
{
    public class Audit
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
