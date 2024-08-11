using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Cabinet
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Location { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public IList<BookShelf> BookShelves { get; set; }
    }
}
