using Microsoft.EntityFrameworkCore;
using SimplexTask.Contracts;
using SimplexTask.DataAccess;
using SimplexTask.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.Include(x => x.Books).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetByFilter(string email, int filter)
        {
            return await _context.Users.Include(x => x.Books.Where(b => b.Rating == filter)).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetById(string id)
        {
            return await _context.Users.Include(x => x.Books).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
