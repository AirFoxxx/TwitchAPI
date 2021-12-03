using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Models
{
    public class App
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Token { get; set; }

        [MaxLength(100)]
        public string RefreshToken { get; set; }

        public TimeSpan ExpiresIn { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ClientSecret { get; set; }

        [Required]
        public string RedirectURI { get; set; }
    }
}
