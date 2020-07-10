// Inspired by https://github.com/dotnet/aspnetcore/blob/master/src/Identity/EntityFrameworkCore/src/UserStore.cs
// and https://github.com/roadkillwiki/Marten.AspNetIdentity/blob/master/src/Marten.AspNetIdentity/MartenUserStore.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Marten.AspNetCore.Identity
{
    public sealed class UserStore<TUser>
     // : IUserStore<TUser>
     : IQueryableUserStore<TUser>,
       IUserAuthenticatorKeyStore<TUser>,
       IUserClaimStore<TUser>,
       IUserEmailStore<TUser>,
       IUserLockoutStore<TUser>,
       IUserLoginStore<TUser>,
       IUserPasswordStore<TUser>,
       IUserPhoneNumberStore<TUser>,
       IUserRoleStore<TUser>,
       IUserSecurityStampStore<TUser>,
       IUserTwoFactorRecoveryCodeStore<TUser>,
       IUserTwoFactorStore<TUser>
    where TUser : IdentityUser<Guid>
    {
        private bool _disposed;

        public UserStore()
        {
            _disposed = false;
        }

        IQueryable<TUser> IQueryableUserStore<TUser>.Users => throw new NotImplementedException();

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

        Task IUserClaimStore<TUser>.AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserLoginStore<TUser>.AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserRoleStore<TUser>.AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<int> IUserTwoFactorRecoveryCodeStore<TUser>.CountCodesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<TUser>.CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<TUser>.DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TUser> IUserEmailStore<TUser>.FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TUser> IUserStore<TUser>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TUser> IUserLoginStore<TUser>.FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<TUser> IUserStore<TUser>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<int> IUserLockoutStore<TUser>.GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserAuthenticatorKeyStore<TUser>.GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IList<Claim>> IUserClaimStore<TUser>.GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserEmailStore<TUser>.GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserEmailStore<TUser>.GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserLockoutStore<TUser>.GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<DateTimeOffset?> IUserLockoutStore<TUser>.GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IList<UserLoginInfo>> IUserLoginStore<TUser>.GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserEmailStore<TUser>.GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserStore<TUser>.GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserPasswordStore<TUser>.GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserPhoneNumberStore<TUser>.GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserPhoneNumberStore<TUser>.GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IList<string>> IUserRoleStore<TUser>.GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserSecurityStampStore<TUser>.GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserTwoFactorStore<TUser>.GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserStore<TUser>.GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<string> IUserStore<TUser>.GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IList<TUser>> IUserClaimStore<TUser>.GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IList<TUser>> IUserRoleStore<TUser>.GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserPasswordStore<TUser>.HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<int> IUserLockoutStore<TUser>.IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserRoleStore<TUser>.IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<bool> IUserTwoFactorRecoveryCodeStore<TUser>.RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserClaimStore<TUser>.RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserRoleStore<TUser>.RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserLoginStore<TUser>.RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserClaimStore<TUser>.ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserTwoFactorRecoveryCodeStore<TUser>.ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<TUser>.ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserAuthenticatorKeyStore<TUser>.SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserEmailStore<TUser>.SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserEmailStore<TUser>.SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<TUser>.SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserLockoutStore<TUser>.SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserEmailStore<TUser>.SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserStore<TUser>.SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserPasswordStore<TUser>.SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserPhoneNumberStore<TUser>.SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserPhoneNumberStore<TUser>.SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserSecurityStampStore<TUser>.SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserTwoFactorStore<TUser>.SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task IUserStore<TUser>.SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }

        Task<IdentityResult> IUserStore<TUser>.UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            throw new NotImplementedException();
        }
    }
}