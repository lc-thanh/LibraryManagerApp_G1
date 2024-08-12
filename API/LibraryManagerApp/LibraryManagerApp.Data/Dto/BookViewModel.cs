using LibraryManagerApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class BookViewModel
    {
        public string Title { get; set; }

        public string? Publisher { get; set; }

        public int? PublishedYear { get; set; }

        public int Quantity { get; set; } = 0;

        public int AvailableQuantity { get; set; } = 0;

        public int TotalPages { get; set; } = 1;

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? AuthorId { get; set; }
        public string AuthorName { get; set; }

        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public Guid? BookShelfId { get; set; }
        public string BookShelfName { get; set; }
    }
}
