using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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

        public async Task<Loan?> FindByCodeAsync(string code)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.LoanCode.ToLower().Equals(code.ToLower()));

            return loan;
        }

        // LoanCode exmaple: ACBXY123
        public string GenerateLoanCode()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            StringBuilder result = new StringBuilder(8);
            Random random = new Random();

            // Sinh 5 ký tự chữ hoa
            for (int i = 0; i < 5; i++)
            {
                result.Append(letters[random.Next(letters.Length)]);
            }

            // Sinh 3 ký tự chữ số
            for (int i = 0; i < 3; i++)
            {
                result.Append(digits[random.Next(digits.Length)]);
            }

            return result.ToString();
        }

        public IQueryable<Loan> GetAllInforsQuery()
        {
            var loanQuery = _context.Loans
                .Include(l => l.Librarian)
                .Include(l => l.Member)
                .Include(l => l.LoanDetails);

            return loanQuery;
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
