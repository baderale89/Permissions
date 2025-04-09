using Microsoft.EntityFrameworkCore;
using Permissions.Models;

namespace Permissions.Interfaces
{
    public interface IPermissionRepository
    {
        Task AddAsync(Permission permission);
        Task<Permission> GetByIdAsync(int id);
        void Update(Permission permission);
        Task<IEnumerable<Permission>> GetAllAsync();
    }
}
