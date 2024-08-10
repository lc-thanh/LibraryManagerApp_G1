using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class AuthorCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
