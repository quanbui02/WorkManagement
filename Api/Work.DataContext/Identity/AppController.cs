using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Work.DataContext.Identity
{
    public class AppController
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Route { get; set; }
        public int? Index { get; set; }
        public int? ServiceId { get; set; }
        public string GroupName { get; set; }
        public ICollection<AppPermission> AppPermission { get; set; }
    }
}
                                                      