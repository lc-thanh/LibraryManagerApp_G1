using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerApp.Data.Dto
{
    public class LoanCreateModel
    {
        [Required]
        public string MemberEmail { get; set; }

        [Required]
        public List<Guid> BookIds { get; set; }

        [Required]
        public DateTime LoanDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime DueDate { get; set; }
    }
}
