using SimplexTask.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.Contracts
{
    public interface IBookRepository
    {
        Task<Book> Add(Book book);
        Task<ICollection<Book>> GetAll();
        Task<ICollection<Book>> GetByFilter(string filter);
        Task<Book> GetById(Guid Id);
        Task<bool> Remove(Guid Id);
        Task<bool> Update(Book book);
    }
}
