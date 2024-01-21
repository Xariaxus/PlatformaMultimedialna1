using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PlatformaMultimedialna.Models
{
    public class MediaModel
    {
       
        public int Id { get; set; }

        public string NazwaPliku { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        public string UserId { get; set; }

    }
}
