using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    }
}
