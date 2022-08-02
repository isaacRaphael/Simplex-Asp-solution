using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Models.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }

    }
}
