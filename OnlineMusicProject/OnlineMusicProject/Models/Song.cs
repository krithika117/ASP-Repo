using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineMusicProject.Models
{
    public class Song
    {
       
            public int Id { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public string Artist { get; set; }

            [Required]
            public int Year { get; set; }

            [Required]
            public string Genre { get; set; }

            [Required]
            public string YouTubeLink { get; set; }
        
    }
}
