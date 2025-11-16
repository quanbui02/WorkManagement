using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Work.DataContext.Identity;

namespace Work.DataContext
{
    public class AppRole : IdentityRole
    {
        public string Code { get; set; }
        public bool AutoAssign { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<AppGrantedPermissions> AppGrantedPermissions { get; set; }
    }
}
