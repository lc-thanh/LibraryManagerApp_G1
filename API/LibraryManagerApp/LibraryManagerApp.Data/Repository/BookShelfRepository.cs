using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Repository
{
    public class BookShelfRepository : BaseRepository<BookShelf>, IBookShelfRepository
    {
        public BookShelfRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
    }
}
