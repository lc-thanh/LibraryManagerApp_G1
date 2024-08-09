using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using LibraryManagerApp.Data.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagerApp.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryManagerAppDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(LibraryManagerAppDbContext context)
        {
            _context = context;
        }

        public LibraryManagerAppDbContext Context => _context;

        public IBaseRepository<Admin> AdminRepository => new AdminRepository(_context);

        public IBaseRepository<Author> AuthorRepository => new AuthorRepository(_context);

        public IBaseRepository<Book> BookRepository => new BookRepository(_context);

        public IBaseRepository<BookShelf> BookShelfRepository => new BookShelfRepository(_context);

        public IBaseRepository<Cabinet> CabinetRepository => new CabinetRepository(_context);

        public IBaseRepository<Category> CategoryRepository => new CategoryRepository(_context);

        public IBaseRepository<Librarian> LibrarianRepository => new LibrarianRepository(_context);

        public IBaseRepository<Loan> LoanRepository => new LoanRepository(_context);

        public IBaseRepository<LoanDetail> LoanDetailRepository => new LoanDetailRepository(_context);

        public IBaseRepository<Member> MemberRepository => new MemberRepository(_context);

        public IBaseRepository<T> BaseRepository<T>() where T : class
        {
            return new BaseRepository<T>(_context);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await RollbackTransactionAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
