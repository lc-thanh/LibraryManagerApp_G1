using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class CabinetCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }
    }
}
