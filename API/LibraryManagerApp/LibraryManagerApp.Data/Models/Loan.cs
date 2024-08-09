using LibraryManagerApp.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Loan
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime ReturnedDate { get; set; }

        [Required]
        public StatusEnum Status { get; set; }

        public Guid MemberId { get; set; }
        public Member Member { get; set; }

        public IList<LoanDetail> LoanDetails { get; set; }
    }
}
