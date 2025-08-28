using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class GnuPgKeyFingerprintType
    : EntityType<GnuPgKeyFingerprint, GnuPgKeyFingerprintByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(f => f.InstitutionId).Ignore();
        descriptor
            .Field(f => f.Institution)
            .Type<NonNullType<ObjectType<GnuPgKeyFingerprintInstitutionEdge>>>()
            .Resolve(context =>
                new GnuPgKeyFingerprintInstitutionEdge(
                    context.Parent<GnuPgKeyFingerprint>()
                )
            );
        descriptor.Field(f => f.UserId).Ignore();
        descriptor
            .Field(f => f.User)
            .Type<NonNullType<ObjectType<GnuPgKeyFingerprintUserEdge>>>()
            .Resolve(context =>
                new GnuPgKeyFingerprintUserEdge(
                    context.Parent<GnuPgKeyFingerprint>()
                )
            );
        descriptor
            .Field("canCurrentUserAllowNode")
            .ResolveWith<GnuPgKeyFingerprintResolvers>(x =>
                GnuPgKeyFingerprintResolvers.GetCanCurrentUserAllowNodeAsync(default!, default!, default!, default!)
            )
            .UseUserManager();
        descriptor
            .Field("canCurrentUserRevokeNode")
            .ResolveWith<GnuPgKeyFingerprintResolvers>(x =>
                GnuPgKeyFingerprintResolvers.GetCanCurrentUserRevokeNodeAsync(default!, default!, default!, default!)
            )
            .UseUserManager();
    }

    private sealed class GnuPgKeyFingerprintResolvers
    {
        public static Task<bool> GetCanCurrentUserAllowNodeAsync(
            [Parent] GnuPgKeyFingerprint gnuPgKeyFingerprint,
            ClaimsPrincipal claimsPrincipal,
            GnuPgKeyFingerprintAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToAllow(claimsPrincipal, gnuPgKeyFingerprint, cancellationToken);
        }

        public static Task<bool> GetCanCurrentUserRevokeNodeAsync(
            [Parent] GnuPgKeyFingerprint gnuPgKeyFingerprint,
            ClaimsPrincipal claimsPrincipal,
            GnuPgKeyFingerprintAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToRevoke(claimsPrincipal, gnuPgKeyFingerprint, cancellationToken);
        }
    }
}