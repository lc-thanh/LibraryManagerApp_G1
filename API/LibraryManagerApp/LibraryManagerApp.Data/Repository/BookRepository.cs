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

        public IQueryable<Book> GetAllInforsQuery()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.BookShelf);

            return books;
        }

        public async Task<IEnumerable<Book>> SearchAsync(string query)
        {
            return await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Name.Contains(query) || b.Description.Contains(query))
                .ToListAsync();
        }
    }
}
