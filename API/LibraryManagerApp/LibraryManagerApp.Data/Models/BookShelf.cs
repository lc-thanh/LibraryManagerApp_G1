using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class BookShelf
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public IList<Book> Books { get; set; }

        public Guid CabinetId { get; set; }
        public Cabinet Cabinet { get; set; }
    }
}
