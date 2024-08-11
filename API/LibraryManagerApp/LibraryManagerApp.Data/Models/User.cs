using LibraryManagerApp.Data.Enum;
using LibraryManagerApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagerApp.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress] // Validate Email
        [MaxLength(255)]
        public string Email { get; set; }

        [MaxLength(12)]
        [RegularExpression(@"^(03|05|07|08|09|01[2|6|8|9])([0-9]{8})$", ErrorMessage = "PhoneNumber is not in correct format!")]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Password { get; set; } // Hashed password

        [Required]
        public RoleEnum Role { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
