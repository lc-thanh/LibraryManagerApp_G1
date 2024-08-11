using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagerApp.Data.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Book>> SearchAsync(string query)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Name.Contains(query) || b.Description.Contains(query))
                .ToListAsync();
        }
    }
}
