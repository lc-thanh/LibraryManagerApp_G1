using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;

namespace LibraryManagerApp.Data.Repository
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        public LoanRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
        public async Task CreateLoanAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<Loan> GetLoanByIdAsync(Guid loanId)
        {
            return await _context.Loans
            .Include(l => l.LoanDetails)
            .ThenInclude(ld => ld.Book)
            .FirstOrDefaultAsync(l => l.Id == loanId);
        }

        public async Task ReturnBooksAsync(Guid loanId)
        {
            var loan = await GetLoanByIdAsync(loanId);
            if (loan != null)
            {
                loan.ReturnedDate = DateTime.Now;
                _context.Loans.Update(loan);

                // Optionally, update book quantities if needed
                foreach (var detail in loan.LoanDetails)
                {
                    var book = await _context.Books.FindAsync(detail.BookId);
                    if (book != null)
                    {
                        book.AvailableQuantity++;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
