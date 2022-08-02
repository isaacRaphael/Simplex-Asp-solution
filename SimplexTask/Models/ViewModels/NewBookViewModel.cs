using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models.ViewModels
{
    public class NewBookViewModel
    {
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be from 1 to 5")]
        public int Rating { get; set; }
        public string UserEmail { get; set; }

    }
}
