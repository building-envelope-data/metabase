using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using GreenDonut.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.Extensions;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.DataFormats;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.GnuPgKeyFingerprints;
using Metabase.GraphQl.InstitutionMethodDevelopers;
using Metabase.GraphQl.InstitutionRepresentatives;
using Metabase.GraphQl.OpenIdConnect.Applications;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

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
            .Type<NonNullType<ObjectType<InstitutionDevelopedMethodConnection>>>()
            .UseFiltering<InstitutionDevelopedMethodFilterType>()
            .Resolve(context =>
                new InstitutionDevelopedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<InstitutionMethodDeveloper>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.DevelopedMethods)}")
            .Type<NonNullType<ObjectType<PendingInstitutionDevelopedMethodConnection>>>()
            .UseFiltering<InstitutionDevelopedMethodFilterType>()
            .Resolve(context =>
                new PendingInstitutionDevelopedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<InstitutionMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.DevelopedMethodEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManufacturedComponents)
            .Type<NonNullType<ObjectType<InstitutionManufacturedComponentConnection>>>()
            .UseFiltering<InstitutionManufacturedComponentFilterType>()
            .Resolve(context =>
                new InstitutionManufacturedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.ManufacturedComponents)}")
            .Type<NonNullType<ObjectType<PendingInstitutionManufacturedComponentConnection>>>()
            .UseFiltering<InstitutionManufacturedComponentFilterType>()
            .Resolve(context =>
                new PendingInstitutionManufacturedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field(t => t.ManufacturedComponentEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManagedComponents)
            .Type<NonNullType<ObjectType<InstitutionManagedComponentConnection>>>()
            .UseFiltering<InstitutionManagedComponentFilterType>()
            .Resolve(context =>
                new InstitutionManagedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<Component>()
                )
            );
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
            .Type<NonNullType<ObjectType<InstitutionRepresentativeConnection>>>()
            // .UseProjection<InstitutionRepresentative>()
            .UseFiltering<InstitutionRepresentativeFilterType>()
            // .UseSorting<InstitutionRepresentativeSortType>()
            .Resolve(context =>
                new InstitutionRepresentativeConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<InstitutionRepresentative>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.Representatives)}")
            .Type<ObjectType<PendingInstitutionRepresentativeConnection>>()
            .Authorize(AuthorizationPolicies.ManageInstitutionRepresentativePolicy)
            // .UseProjection<InstitutionRepresentative>()
            .UseFiltering<InstitutionRepresentativeFilterType>()
            // .UseSorting<InstitutionRepresentativeSortType>()
            .Resolve(context =>
                new PendingInstitutionRepresentativeConnection(
                    context.Parent<Institution>(),
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
            .Type<NonNullType<ObjectType<InstitutionGnuPgKeyFingerprintConnection>>>()
            .UseFiltering<InstitutionGnuPgKeyFingerprintFilterType>()
            .Resolve(context =>
                new InstitutionGnuPgKeyFingerprintConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<GnuPgKeyFingerprint>()
                )
            );
        descriptor
            .Field("has" + nameof(GnuPgKeyFingerprint))
            .UseFiltering<InstitutionGnuPgKeyFingerprintFilterType>()
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.HasGnuPgKeyFingerprintsAsync(default!, default!, default!, default!));
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToDeleteNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToDeleteNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToSwitchOperatingStateOfNode")
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToSwitchOperatingStateOfNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class InstitutionResolvers
    {
        public static Task<bool> HasGnuPgKeyFingerprintsAsync(
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

        public static Task<bool> IsAuthorizedToUpdateNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToUpdateInstitution(claimsPrincipal, institution.Id, cancellationToken);
        }

        public static Task<bool> IsAuthorizedToDeleteNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToDeleteInstitution(claimsPrincipal, institution.Id, cancellationToken);
        }

        public static Task<bool> IsAuthorizedToSwitchOperatingStateOfNodeAsync(
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