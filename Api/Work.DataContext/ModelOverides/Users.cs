using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext.Models
{
    public partial class Users
    {
        public string? IdentityUserId { get; set; }
        public virtual AppUser IdentityUser { get; set; }
    }
}
