using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Data.OpenIdConnect;
using Metabase.GraphQl.Components;
using Metabase.GraphQl.DataFormats;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Users;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionType
    : EntityType<Institution, IInstitutionByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.DevelopedMethods)
            .Type<NonNullType<ObjectType<InstitutionDevelopedMethodConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionDevelopedMethodFilterType>()
            .UseSorting<InstitutionDevelopedMethodSortType>()
            .Resolve(context =>
                new InstitutionDevelopedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<InstitutionMethodDeveloper>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.DevelopedMethods)}")
            .Type<NonNullType<ObjectType<PendingInstitutionDevelopedMethodConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionDevelopedMethodFilterType>()
            .UseSorting<InstitutionDevelopedMethodSortType>()
            .Resolve(context =>
                new PendingInstitutionDevelopedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<InstitutionMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.DevelopedMethodEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManufacturedComponents)
            .Type<NonNullType<ObjectType<InstitutionManufacturedComponentConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManufacturedComponentFilterType>()
            .UseSorting<InstitutionManufacturedComponentSortType>()
            .Resolve(context =>
                new InstitutionManufacturedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.ManufacturedComponents)}")
            .Type<NonNullType<ObjectType<PendingInstitutionManufacturedComponentConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManufacturedComponentFilterType>()
            .UseSorting<InstitutionManufacturedComponentSortType>()
            .Resolve(context =>
                new PendingInstitutionManufacturedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field(t => t.ManufacturedComponentEdges)
            .Ignore();
        descriptor
            .Field(t => t.ManagedComponents)
            .Type<NonNullType<ObjectType<InstitutionManagedComponentConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManagedComponentFilterType>()
            .UseSorting<InstitutionManagedComponentSortType>()
            .Resolve(context =>
                new InstitutionManagedComponentConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<Component>()
                )
            );
        descriptor
            .Field(t => t.ManagedDataFormats)
            .Type<NonNullType<ObjectType<InstitutionManagedDataFormatConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManagedDataFormatFilterType>()
            .UseSorting<InstitutionManagedDataFormatSortType>()
            .Resolve(context =>
                new InstitutionManagedDataFormatConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<DataFormat>()
                )
            );
        descriptor
            .Field(t => t.ManagedInstitutions)
            .Type<NonNullType<ObjectType<InstitutionManagedInstitutionConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManagedInstitutionFilterType>()
            .UseSorting<InstitutionManagedInstitutionSortType>()
            .Resolve(context =>
                new InstitutionManagedInstitutionConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
                    context.GetQueryContext<Institution>()
                )
            );
        descriptor
            .Field(t => t.ManagedMethods)
            .Type<NonNullType<ObjectType<InstitutionManagedMethodConnection>>>()
            .AddPagingArguments()
            .UseFiltering<InstitutionManagedMethodFilterType>()
            .UseSorting<InstitutionManagedMethodSortType>()
            .Resolve(context =>
                new InstitutionManagedMethodConnection(
                    context.Parent<Institution>(),
                    context.GetPagingArguments(),
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
            .UseSorting<InstitutionOperatedDatabaseSortType>()
            .Resolve(context =>
                new InstitutionOperatedDatabaseConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<Database>()
                )
            );
        descriptor
            .Field(t => t.Representatives)
            .Type<NonNullType<ObjectType<InstitutionRepresentativeConnection>>>()
            .UseFiltering<InstitutionRepresentativeFilterType>()
            .UseSorting<InstitutionRepresentativeSortType>()
            .Resolve(context =>
                new InstitutionRepresentativeConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<InstitutionRepresentative>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Institution.Representatives)}")
            .Type<ObjectType<PendingInstitutionRepresentativeConnection>>()
            .Authorize(AuthorizationPolicies.ManageInstitutionRepresentativeScopePolicy)
            .UseFiltering<InstitutionRepresentativeFilterType>()
            .UseSorting<InstitutionRepresentativeSortType>()
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
            .UseSorting<InstitutionOwnedOpenIdConnectApplicationSortType>()
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
            .UseSorting<InstitutionGnuPgKeyFingerprintSortType>()
            .Resolve(context =>
                new InstitutionGnuPgKeyFingerprintConnection(
                    context.Parent<Institution>(),
                    context.GetQueryContext<GnuPgKeyFingerprint>()
                )
            );
        descriptor
            .Field("has" + nameof(GnuPgKeyFingerprint))
            .UseFiltering<InstitutionGnuPgKeyFingerprintFilterType>()
            .UseSorting<InstitutionGnuPgKeyFingerprintSortType>()
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.HasGnuPgKeyFingerprintsAsync(default!, default!, default!, default!));
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToVerifyNode")
            .Cost(1)
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToVerifyNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToDeleteNode")
            .Cost(1)
            .ResolveWith<InstitutionResolvers>(x =>
                InstitutionResolvers.IsAuthorizedToDeleteNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
        descriptor
            .Field("isAuthorizedToSwitchOperatingStateOfNode")
            .Cost(1)
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

        public static Task<bool> IsAuthorizedToVerifyNodeAsync(
            [Parent] Institution institution,
            ClaimsPrincipal claimsPrincipal,
            InstitutionAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToVerifyInstitution(claimsPrincipal, cancellationToken);
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

        // internal static Task<Connection<Component>> GetManufacturedComponentsAsync(
        //     [Parent] Component component,
        //     IInstitutionManufactureredComponentsByInstitutionIdDataLoader dataLoader,
        //     IComponentByIdDataLoader nodeById,
        //     ApplicationDbContext databaseContext,
        //     PagingArguments pagingArguments,
        //     QueryContext<ComponentManufacturer> queryContext,
        //     QueryContext<Component> nodeQueryContext,
        //     CancellationToken cancellationToken
        // )
        // {
        //     return dataLoader
        //         .With(pagingArguments, queryContext)
        //         .LoadAsync(institution.Id, cancellationToken)
        //         .ToConnectionAsync<ComponentManufacturer, Component>(
        //             async (page, entry) =>
        //             {
        //                 return new InstitutionManufactureredComponentEdge(
        //                     entry.Item,
        //                     await nodeById
        //                         .With(nodeQueryContext)
        //                         .LoadAsync(entry.Item.ComponentId, cancellationToken),
        //                     page.CreateCursor(entry)
        //                 );
        //             },
        //             (edges, pageInfo, totalCount) =>
        //                 new InstitutionManufacturedComponentConnection(institution, edges, pageInfo, totalCount)
        //         );
        // }
    }
}