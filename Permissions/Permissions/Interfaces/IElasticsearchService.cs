using Permissions.Models;

namespace Permissions.Interfaces
{
    public interface IElasticsearchService
    {
        Task IndexPermissionAsync(Permission permission);
    }
}
