﻿using LibraryManagerApp.Data.Models;

namespace LibraryManagerApp.Data.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        IQueryable<Book> GetAllInforsQuery();
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
    }
}
