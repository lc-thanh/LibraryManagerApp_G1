using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class BookCreateModel
    {
        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string? Publisher { get; set; }

        [Range(1,int.MaxValue)]
        public int? PublishedYear { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Range(1,int.MaxValue)]
        public int TotalPages {  get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        public Guid? AuthorId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? BookShelfId { get; set; }
    }
}
