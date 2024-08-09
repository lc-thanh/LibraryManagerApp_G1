using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Repository
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
    }
}
