using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        LibraryManagerAppDbContext Context { get; }
        IBaseRepository<Admin> AdminRepository { get; }
        IBaseRepository<Author> AuthorRepository { get; }
        IBaseRepository<Book> BookRepository { get; }
        IBaseRepository<BookShelf> BookShelfRepository { get; }
        IBaseRepository<Cabinet> CabinetRepository { get; }
        IBaseRepository<Category> CategoryRepository { get; }
        IBaseRepository<Librarian> LibrarianRepository { get; }
        IBaseRepository<Loan> LoanRepository { get; }
        IBaseRepository<LoanDetail> LoanDetailRepository { get; }
        IBaseRepository<Member> MemberRepository { get; }
        IBaseRepository<T> BaseRepository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
