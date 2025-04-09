using MediatR;
namespace Permissions.Events.Commands
{
    public class RequestPermissionCommand : IRequest<int>
    {
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public int PermissionTypeId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
