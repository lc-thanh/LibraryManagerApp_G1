using System.ComponentModel.DataAnnotations;

namespace LibraryManagerApp.Data.Models
{
    public class Member : User
    {
        public int GetMembershipDays()
        {
            TimeSpan difference = DateTime.Now - CreatedOn;
            return difference.Days;
        }

        public IList<Loan> Loans { get; set; }
    }
}
