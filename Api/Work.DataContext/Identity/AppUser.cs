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
        public bool IsSuperUser { get; set; }
    }
}
