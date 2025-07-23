using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext.Identity
{
    public class AppPermission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Service { get; set; }
        public string DisplayName { get; set; }
        public string Route { get; set; }
        public string Controller { get; set; }
        public string PermissionCode { get; set; }
        public int? Index { get; set; }
        public string Description { get; set; }
        public int AppControllerId { get; set; }
        public AppController AppController { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
