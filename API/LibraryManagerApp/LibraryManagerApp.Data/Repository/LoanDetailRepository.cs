using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Repository
{
    public class LoanDetailRepository : BaseRepository<LoanDetail>, ILoanDetailRepository
    {
        public LoanDetailRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
    }
}
