using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        LibraryManagerAppDbContext Context { get; }
        IAdminRepository AdminRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IBookRepository BookRepository { get; }
        IBookShelfRepository BookShelfRepository { get; }
        ICabinetRepository CabinetRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ILibrarianRepository LibrarianRepository { get; }
        ILoanRepository LoanRepository { get; }
        ILoanDetailRepository LoanDetailRepository { get; }
        IMemberRepository MemberRepository { get; }
        IUserRepository UserRepository { get; }
        IBaseRepository<T> BaseRepository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
