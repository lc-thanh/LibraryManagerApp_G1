using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        public int? PublishedYear { get; set; }

        [Required]
        public int Quantity { get; set; } = 0;

        [Required]
        public int AvailableQuantity { get; set; } = 0;
        
        [Required]
        public int TotalPages { get; set; } = 1;

        [MaxLength(512)]
        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public Guid? AuthorId { get; set; }
        public Author Author { get; set; }

        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid? BookShelfId { get; set; }
        public BookShelf BookShelf { get; set; }

        public IList<LoanDetail> LoanDetails { get; set; }
    }
}
