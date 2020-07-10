// Inspired by https://github.com/dotnet/aspnetcore/blob/master/src/Identity/EntityFrameworkCore/src/RoleStore.cs
// and https://github.com/roadkillwiki/Marten.AspNetIdentity/blob/master/src/Marten.AspNetIdentity/MartenRoleStore.cs

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Marten.AspNetCore.Identity
{
    public class RoleStore<TRole>
     // : IRoleStore<TRole>,
     : IQueryableRoleStore<TRole>
     where TRole : IdentityRole<Guid>
    {
        private bool _disposed;

        IQueryable<TRole> IQueryableRoleStore<TRole>.Roles => throw new NotImplementedException();

        public RoleStore()
        {
            _disposed = false;
        }

        void IDisposable.Dispose()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose?view=netcore-3.1#examples
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        Task<IdentityResult> IRoleStore<TRole>.CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IdentityResult> IRoleStore<TRole>.DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TRole> IRoleStore<TRole>.FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TRole> IRoleStore<TRole>.FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<TRole>.GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<TRole>.GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IRoleStore<TRole>.GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IRoleStore<TRole>.SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IRoleStore<TRole>.SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IdentityResult> IRoleStore<TRole>.UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }
    }
}