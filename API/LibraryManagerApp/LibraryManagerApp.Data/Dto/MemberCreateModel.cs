using LibraryManagerApp.Data.Enum;
using LibraryManagerApp.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class MemberCreateModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress] // Validate Email
        [MaxLength(255)]
        public string Email { get; set; }

        [StringLength(12, MinimumLength = 10)]
        public string Phone { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        [PasswordStrength] // Validate password
        public string Password { get; set; }

        [Required]
        public RoleEnum Role { get; set; }

        public DateTime MembershipDate { get; set; } = DateTime.Now;
    }
}
