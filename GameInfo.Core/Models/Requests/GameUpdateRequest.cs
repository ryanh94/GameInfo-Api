using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GameInfo.Core.Models.Requests
{
    public class GameUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; }
    }
}
