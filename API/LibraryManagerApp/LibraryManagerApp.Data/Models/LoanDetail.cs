using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class LoanDetail
    {
        public Guid LoanId { get; set; }
        public Loan Loan { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }

        [Required]
        public int Quantity { get; set; } // Quantity of borrow book
    }
}
