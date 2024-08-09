using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Repository
{
    public class LibrarianRepository : BaseRepository<Librarian>, ILibrarianRepository
    {
        public LibrarianRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
    }
}
