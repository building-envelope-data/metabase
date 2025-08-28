using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.InstitutionRepresentatives;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.DataFormats;
using Metabase.GraphQl.InstitutionMethodDevelopers;
using Metabase.GraphQl.OpenIdConnect.Applications;
using Metabase.Data.OpenIdConnect;
using System.Collections.Generic;
using GreenDonut;
using HotChocolate.Resolvers;
using Microsoft.EntityFrameworkCore;
using HotChocolate.Data;
using System.Linq;
using Metabase.GraphQl.GnuPgKeyFingerprints;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionType
    : EntityType<Institution, InstitutionByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.DevelopedMethods)
            .Argument(nameof(InstitutionMethodDeveloper.Pending).FirstCharToLower(),
                _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
            .Type<NonNullType<ObjectType<InstitutionDevelopedMethodConnection>>>()
            .UseFiltering<InstitutionDevelopedMethodFilterType>()
            .Resolve(context =>
                new InstitutionDevelopedMethodConnection(
                    context.Parent<Institution>(),
                    context.ArgumentValue<bool>(nameof(InstitutionMethodDeveloper.Pending).FirstCharToLower()),
                    context.GetQueryContext<InstitutionMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.DevelopedMethodEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManufacturedComponents)
            .Argument(nameof(ComponentManufacturer.Pending).FirstCharToLower(),
                _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
            .Type<NonNullType<ObjectType<InstitutionManufacturedComponentConnection>>>()
            .UseFiltering<InstitutionManufacturedComponentFilterType>()
            .Resolve(context =>
                new InstitutionManufacturedComponentConnection(
                    context.Parent<Institution>(),
                    context.ArgumentValue<bool>(nameof(ComponentManufacturer.Pending).FirstCharToLower()),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field(t => t.ManufacturedComponentEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManagedDataFormats)
            .Type<NonNullType<ObjectType<InstitutionManagedDataFormatConnection>>>()
            .UseFiltering<InstitutionManagedDataFormatFilterType>()
            .Resolve(context =>
                new InstitutionManagedDataFormatConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<DataFormat>()
                )
            );
        descriptor
            .Field(t => t.ManagedInstitutions)
            .Type<NonNullType<ObjectType<InstitutionManagedInstitutionConnection>>>()
            .UseFiltering<InstitutionManagedInstitutionFilterType>()
            .Resolve(context =>
                new InstitutionManagedInstitutionConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<Institution>()
                )
            );
        descriptor
            .Field(t => t.ManagedMethods)
            .Type<NonNullType<ObjectType<InstitutionManagedMethodConnection>>>()
            .UseFiltering<InstitutionManagedMethodFilterType>()
            .Resolve(context =>
                new InstitutionManagedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<Method>()
                )
            );
        descriptor
            .Field(t => t.Manager)
            .Type<ObjectType<InstitutionManagerEdge>>()
            .Resolve(context =>
                {
                    var institution = context.Parent<Institution>();
                    return institution.ManagerId is null
                        ? null
                        : new InstitutionManagerEdge(institution);
                }
            );
        descriptor
            .Field(t => t.ManagerId)
            .Ignore();
        descriptor
            .Field(t => t.OperatedDatabases)
            .Type<NonNullType<ObjectType<InstitutionOperatedDatabaseConnection>>>()
            .UseFiltering<InstitutionOperatedDatabaseFilterType>()
            .Resolve(context =>
                new InstitutionOperatedDatabaseConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<Database>()
                )
            );
        descriptor
            .Field(t => t.Representatives)
            .Argument(nameof(InstitutionRepresentative.Pending).FirstCharToLower(),
                _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
            .Type<NonNullType<ObjectType<InstitutionRepresentativeConnection>>>()
            // .UseProjection<InstitutionRepresentative>()
            .UseFiltering<InstitutionRepresentativeFilterType>()
            // .UseSorting<InstitutionRepresentativeSortType>()
            .Resolve(context =>
                new InstitutionRepresentativeConnection(
                    context.Parent<Institution>(),
                    context.ArgumentValue<bool>(nameof(InstitutionRepresentative.Pending).FirstCharToLower()),
                    context.GetQueryContext<InstitutionRepresentative>()
                )
            );
        descriptor
            .Field(t => t.RepresentativeEdges)
            .Ignore();
        descriptor
            .Field(t => t.OpenIdConnectApplications)
            .Type<NonNullType<ObjectType<InstitutionOwnedOpenIdConnectApplicationConnection>>>()
            .UseFiltering<InstitutionOwnedOpenIdConnectApplicationFilterType>()
            .Resolve(context =>
                new InstitutionOwnedOpenIdConnectApplicationConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<OpenIdConnectApplication>()
                )
            );
        descriptor
            .Field(t => t.GnuPgKeyFingerprints)
            .UseFiltering<InstitutionGnuPgKeyFingerprintFilterType>()
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.GetGnuPgKeyFingerprintsAsync(default!, default!, default!, default!));
        descriptor
            .Field("Has" + nameof(GnuPgKeyFingerprint))
            .UseFiltering<InstitutionGnuPgKeyFingerprintFilterType>()
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.GetHasGnuPgKeyFingerprintsAsync(default!, default!, default!, default!));
        descriptor
            .Field("canCurrentUserUpdateNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.GetCanCurrentUserUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("canCurrentUserDeleteNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.GetCanCurrentUserDeleteNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("canCurrentUserSwitchOperatingStateOfNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.GetCanCurrentUserSwitchOperatingStateOfNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class InstitutionResolvers
    {
        public static Task<GnuPgKeyFingerprint[]> GetGnuPgKeyFingerprintsAsync(
            [Parent] Institution institution,
            GnuPgKeyFingerprintsByInstitutionIdDataLoader dataLoader,
            QueryContext<GnuPgKeyFingerprint> queryContext,
            CancellationToken cancellationToken
        )
        {
            return dataLoader
                .With(queryContext)
                .LoadRequiredAsync(institution.Id, cancellationToken);
        }

        public static Task<bool> GetHasGnuPgKeyFingerprintsAsync(
            [Parent] Institution institution,
            ApplicationDbContext context,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            return context.GnuPgKeyFingerprints.AsNoTracking()
                .Filter(resolverContext)
                .Where(f => f.InstitutionId == institution.Id)
                .AnyAsync(cancellationToken);
        }

        public static Task<bool> GetCanCurrentUserUpdateNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToUpdateInstitution(claimsPrincipal, institution.Id, cancellationToken);
        }

        public static Task<bool> GetCanCurrentUserDeleteNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToDeleteInstitution(claimsPrincipal, institution.Id, cancellationToken);
        }

        public static Task<bool> GetCanCurrentUserSwitchOperatingStateOfNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToSwitchInstitutionOperatingState(claimsPrincipal, institution.Id, cancellationToken);
        }
    }
}