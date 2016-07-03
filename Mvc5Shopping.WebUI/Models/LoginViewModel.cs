using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc5Shopping.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        public String UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}