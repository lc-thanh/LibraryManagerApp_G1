using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagerApp.Data.Models
{
    public class UserAction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string ActionName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
