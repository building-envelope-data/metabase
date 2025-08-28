using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Institutions;
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
            .ResolveWith<GnuPgKeyFingerprintResolvers>(x =>
                GnuPgKeyFingerprintResolvers.GetInstitutionAsync(default!, default!, default!)
            );
        descriptor.Field(f => f.UserId).Ignore();
        descriptor
            .Field(f => f.User)
            .ResolveWith<GnuPgKeyFingerprintResolvers>(x =>
                GnuPgKeyFingerprintResolvers.GetUserAsync(default!, default!, default!)
            );
    }

    private sealed class GnuPgKeyFingerprintResolvers
    {
        public static async Task<Institution> GetInstitutionAsync(
            [Parent] GnuPgKeyFingerprint gnuPgKeyFingerprint,
            InstitutionByIdDataLoader dataLoader,
            CancellationToken cancellationToken
        )
        {
            return (await dataLoader.LoadAsync(gnuPgKeyFingerprint.InstitutionId, cancellationToken))!;
        }

        public static async Task<User> GetUserAsync(
            [Parent] GnuPgKeyFingerprint gnuPgKeyFingerprint,
            UserByIdDataLoader dataLoader,
            CancellationToken cancellationToken
        )
        {
            return (await dataLoader.LoadAsync(gnuPgKeyFingerprint.UserId, cancellationToken))!;
        }
    }
}