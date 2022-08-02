using SimplexTask.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(string id);
        Task<User> GetByFilter(string email, int filter);
    }
}
