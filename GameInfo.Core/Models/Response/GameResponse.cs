using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models.Response
{
    public class GameResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Description { get; set; }
    }
}
