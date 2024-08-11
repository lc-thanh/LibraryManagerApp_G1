using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        void Remove(Book existingBook);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    }
}
