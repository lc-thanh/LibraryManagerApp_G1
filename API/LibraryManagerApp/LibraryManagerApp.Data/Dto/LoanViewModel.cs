using LibraryManagerApp.Data.Enum;
using LibraryManagerApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Dto
{
    public class LoanViewModel
    {
        public Guid Id { get; set; }
        public string LoanCode { get; set; }

        public DateTime LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnedDate { get; set; }

        public StatusEnum Status { get; set; }

        public Guid MemberId { get; set; }
        public string MemberEmail { get; set; }
        public string MemberFullName { get; set; }

        public Guid LibrarianId { get; set; }
        public string LibrarianFullName { get; set; }

        public IList<string> BookNames { get; set; }
    }
}
