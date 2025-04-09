using MediatR;

namespace Permissions.Events.Commands
{
        public class ModifyPermissionCommand : IRequest<bool>
        {
            public int Id { get; set; }
            public string? EmployeeFirstName { get; set; }
            public string? EmployeeLastName { get; set; }
            public int? PermissionTypeId { get; set; }
            public DateTime? Date { get; set; }
        }
}
