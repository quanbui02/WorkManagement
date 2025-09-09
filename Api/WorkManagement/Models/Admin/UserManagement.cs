namespace WorkManagement.Models
{
    public class UserManagement
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSuperUser { get; set; }
        public bool IsDisable { get; set; }
        public int IdUser { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
