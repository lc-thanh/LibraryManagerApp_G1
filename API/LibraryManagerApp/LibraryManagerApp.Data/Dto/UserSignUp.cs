using LibraryManagerApp.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LibraryManagerApp.Data.Dto
{
    public class UserSignUp
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress] // Validate Email
        [MaxLength(255)]
        public string Email { get; set; }

        [AllowNull]
        [RegularExpression(@"^(03|05|07|08|09|01[2|6|8|9])([0-9]{8})$", ErrorMessage = "PhoneNumber is not in correct format!")]
        public string? Phone { get; set; }

        [Required]
        [PasswordStrength] // Validate password
        public string Password { get; set; }
    }
}
