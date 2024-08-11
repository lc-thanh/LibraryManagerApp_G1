using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Author
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public IList<Book> Books { get; set; }
    }
}
