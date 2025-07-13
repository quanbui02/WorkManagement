using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.DataContext.Models;
using Microsoft.AspNetCore.Identity;

namespace Work.DataContext
{
    public class AppUser : IdentityUser
    {
        // Thêm trường tuỳ ý
        public Users UsersNavigation { get; set; }  // Navigation ngược (nếu cần)
    }
}
