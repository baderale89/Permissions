using MediatR;
using Permissions.Models;
namespace Permissions.Events.Queries
{

    public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
    {
    }
    
}
