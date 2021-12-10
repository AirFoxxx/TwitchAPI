using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.Models.AppUsers
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Twitch user ID")]
        public int TwitchUserId { get; set; }
    }
}
