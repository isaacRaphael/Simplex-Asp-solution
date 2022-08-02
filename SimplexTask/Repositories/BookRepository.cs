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
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Book>  Add(Book book)
        {
            await _context.Books.AddAsync(book);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return book;
            return null;

        }

        public async Task<ICollection<Book>> GetAll()
        {
            return await _context.Books.Include(x => x.User).ToListAsync();
        }

        public async Task<ICollection<Book>> GetByFilter(string filter)
        {
            return await _context.Books.Include(x => x.User).Where(b => b.CreatedDate.ToString("yyyy-MM-dd") == filter).ToListAsync();
        }

        public Task<Book> GetById(Guid Id)
        {
            return _context.Books.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<bool> Remove(Guid Id)
        {
            var book = await _context.Books.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
            _context.Books.Remove(book);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(Book book)
        {
            _context.Update(book);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
