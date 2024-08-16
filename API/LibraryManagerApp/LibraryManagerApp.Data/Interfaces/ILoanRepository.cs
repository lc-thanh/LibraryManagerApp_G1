using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task<Loan?> FindByCodeAsync(string code);
        string GenerateLoanCode();
        Task CreateLoanAsync(Loan loan);
        IQueryable<Loan> GetAllInforsQuery();
        Task<Loan> GetLoanByIdAsync(Guid loanId);
        Task ReturnBooksAsync(Guid loanId);
    }
}
