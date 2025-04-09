using Permissions.Data;
using Permissions.Interfaces;

namespace Permissions.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IPermissionRepository Permissions { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Permissions = new PermissionRepository(context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}