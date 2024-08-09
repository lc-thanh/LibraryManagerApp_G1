using LibraryManagerApp.Data.Data;
using LibraryManagerApp.Data.Interfaces;
using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Repository
{
    public class CabinetRepository : BaseRepository<Cabinet>, ICabinetRepository
    {
        public CabinetRepository(LibraryManagerAppDbContext context) : base(context)
        {
        }
    }
}
