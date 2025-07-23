using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext.Identity
{
    public class RolePermission
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public AppRole Role { get; set; }
        public int AppPermissionId { get; set; }
        public AppPermission AppPermission { get; set; }
    }
}
