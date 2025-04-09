using Microsoft.EntityFrameworkCore;
using Permissions.Data;
using Permissions.Interfaces;
using Permissions.Models;

namespace Permissions.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _context;

        public PermissionRepository(AppDbContext context) => _context = context;

        public async Task AddAsync(Permission permission) => await _context.Permissions.AddAsync(permission);
        public async Task<Permission> GetByIdAsync(int id) => await _context.Permissions.FindAsync(id);
        public void Update(Permission permission) => _context.Permissions.Update(permission);
        public async Task<IEnumerable<Permission>> GetAllAsync() => await _context.Permissions.ToListAsync();
    }
}
