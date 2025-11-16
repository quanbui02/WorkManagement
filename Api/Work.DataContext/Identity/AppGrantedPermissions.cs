using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext.Identity
{
    public class AppGrantedPermissions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PermissionValue { get; set; }
        public string? UserIdGuid { get; set; }
        public int CreatedUserId { get; set; }
        public int LastModifyUserId { get; set; }
        public DateTime? LastModifyDate { get; set; }
        public int? AppControllerId { get; set; }
        public AppController AppController { get; set; }
        public string RoleId { get; set; }
        public AppRole Role { get; set; }
    }
}
