using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAPI.ViewModels
{
    public class ForgottenPasswordVM
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email address is required!")]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }
    }
}
