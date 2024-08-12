using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface ILoanRepository : IBaseRepository<Loan>
    {
        Task CreateLoanAsync(Loan loan);
        Task<Loan> GetLoanByIdAsync(Guid loanId);
        Task ReturnBooksAsync(Guid loanId);
    }
}
