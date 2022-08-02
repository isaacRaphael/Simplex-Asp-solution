using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Must be 5 characters long")]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
