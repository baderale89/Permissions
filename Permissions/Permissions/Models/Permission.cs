namespace Permissions.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string? EmployeeForeName { get; set; }
        public string? EmployeeSurName { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime PermissionDate { get; set; }
        public virtual PermissionType? PermissionType { get; set; }
    }
}
