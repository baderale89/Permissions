namespace Permissions.Models
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string EmployeeForeName { get; set; }
        public string EmployeeSurName { get; set; }
        public int PermissionTypeId { get; set; }
        public string PermissionTypeDescription { get; set; }
        public DateTime Date { get; set; }
    }
    
}
