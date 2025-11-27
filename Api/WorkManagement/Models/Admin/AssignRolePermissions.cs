namespace WorkManagement.Models
{
    public class AssignRolePermissions
    {
        public string? RoleId { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
