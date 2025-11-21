namespace WorkManagement.Models
{
    public class AppRoleRequest
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool AutoAssign { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
