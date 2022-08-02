using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models.Entities
{
    public class User : IdentityUser
    {
        public string Fullname { get; set; }
        public ICollection<Book> Books { get; set; }

    }
}
