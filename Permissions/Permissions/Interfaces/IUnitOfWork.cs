using Permissions.Interfaces;

namespace Permissions.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPermissionRepository Permissions { get; }
        Task<int> CommitAsync();
    }
}