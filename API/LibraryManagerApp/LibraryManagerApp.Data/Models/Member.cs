using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Member : User
    {
        [Required]
        public DateTime MembershipDate { get; set; }

        public IList<Loan> Loans { get; set; }
    }
}
