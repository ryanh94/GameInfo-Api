using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Models.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Deleted { get; set; }
}
}
