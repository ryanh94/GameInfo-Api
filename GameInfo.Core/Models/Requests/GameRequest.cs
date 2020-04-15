using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameInfo.Core.Models.Requests
{
    public class GameRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
