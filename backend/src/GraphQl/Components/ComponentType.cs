using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.CalorimetricDataX;
using Metabase.GraphQl.GeometricDataX;
using Metabase.GraphQl.HygrothermalDataX;
using Metabase.GraphQl.LifeCycleDataX;
using Metabase.GraphQl.OpticalDataX;
using Metabase.GraphQl.PhotovoltaicDataX;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Scalars;
using Metabase.GraphQl.Users;
using Metabase.GraphQl.Requests;
using HotChocolate.CostAnalysis.Types;

namespace Metabase.GraphQl.Components;

public sealed class ComponentType
    : EntityType<Component, IComponentByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.PrimeSurface)
            .Ignore();
        descriptor
            .Field(t => t.PrimeDirection)
            .Ignore();
        descriptor
            .Field("prime")
            .Type<ObjectType<PrimeSurfaceOrDirection>>()
            .Cost(0)
            .Resolve(context =>
            {
                var component = context.Parent<Component>();
                return component.PrimeSurface is null && component.PrimeDirection is null
                    ? null
                    : new PrimeSurfaceOrDirection(
                        component.PrimeSurface,
                        component.PrimeDirection
                      );
            });
        descriptor
            .Field(t => t.Manager)
            .Type<NonNullType<ObjectType<ComponentManagerEdge>>>()
            .Resolve(context =>
                new ComponentManagerEdge(
                    context.Parent<Component>()
                )
            );
        descriptor
            .Field(t => t.ManagerId)
            .Ignore();
        descriptor
            .Field(t => t.Manufacturers)
            .Type<NonNullType<ObjectType<ComponentManufacturerConnection>>>()
            .UseFiltering<ComponentManufacturerFilterType>()
            .UseSorting<ComponentManufacturerSortType>()
            .Resolve(context =>
                new ComponentManufacturerConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Component.Manufacturers)}")
            .Type<ObjectType<PendingComponentManufacturerConnection>>()
            .Authorize(AuthorizationPolicies.WriteScopePolicy)
            .UseFiltering<ComponentManufacturerFilterType>()
            .UseSorting<ComponentManufacturerSortType>()
            .Resolve(context =>
                new PendingComponentManufacturerConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentManufacturer>()
                )
            );
        descriptor
            .Field(t => t.ManufacturerEdges).Ignore();
        descriptor
            .Field(t => t.Parts)
            .Name("assembledOf")
            .Type<NonNullType<ObjectType<ComponentAssembledOfConnection>>>()
            .UseFiltering<ComponentAssembledOfFilterType>()
            .UseSorting<ComponentAssembledOfSortType>()
            .Resolve(context =>
                new ComponentAssembledOfConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentAssembly>()
                )
            );
        descriptor
            .Field(t => t.PartEdges).Ignore();
        descriptor
            .Field(t => t.PartOf)
            .Type<NonNullType<ObjectType<ComponentPartOfConnection>>>()
            .UseFiltering<ComponentPartOfFilterType>()
            .UseSorting<ComponentPartOfSortType>()
            .Resolve(context =>
                new ComponentPartOfConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentAssembly>()
                )
            );
        descriptor
            .Field(t => t.PartOfEdges).Ignore();
        descriptor
            .Field(t => t.Generalizations)
            .Name("concretizationOf")
            .Type<NonNullType<ObjectType<ComponentConcretizationOfConnection>>>()
            .UseFiltering<ComponentConcretizationOfFilterType>()
            .UseSorting<ComponentConcretizationOfSortType>()
            .Resolve(context =>
                new ComponentConcretizationOfConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentConcretizationAndGeneralization>()
                )
            );
        descriptor
            .Field(t => t.GeneralizationEdges)
            .Ignore();
        descriptor
            .Field(t => t.Concretizations)
            .Name("generalizationOf")
            .Type<NonNullType<ObjectType<ComponentGeneralizationOfConnection>>>()
            .UseFiltering<ComponentGeneralizationOfFilterType>()
            .UseSorting<ComponentGeneralizationOfSortType>()
            .Resolve(context =>
                new ComponentGeneralizationOfConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentConcretizationAndGeneralization>()
                )
            );
        descriptor
            .Field(t => t.ConcretizationEdges)
            .Ignore();
        descriptor
            .Field(t => t.Variants)
            .Ignore();
        descriptor
            .Field(t => t.VariantEdges).Ignore();
        descriptor
            .Field(t => t.VariantOf)
            .Type<NonNullType<ObjectType<ComponentVariantOfConnection>>>()
            .UseFiltering<ComponentVariantOfFilterType>()
            .UseSorting<ComponentVariantOfSortType>()
            .Resolve(context =>
                new ComponentVariantOfConnection(
                    context.Parent<Component>(),
                    context.GetQueryContext<ComponentVariant>()
                )
            );
        descriptor
            .Field(t => t.VariantOfEdges).Ignore();
        descriptor
            .Field("allCalorimetricData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllCalorimetricDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("allGeometricData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllGeometricDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("allHygrothermalData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllHygrothermalDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("allLifeCycleData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllLifeCycleDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("allOpticalData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllOpticalDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("allPhotovoltaicData")
            .ResolveWith<ComponentResolvers>(_ => _.GetAllPhotovoltaicDataAsync(default!, default, default, default, default, default, default!, default!, default));
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<ComponentResolvers>(x =>
                ComponentResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class ComponentResolvers(
        DataQueries dataQueries
    )
    {
        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<CalorimetricDataConnection> GetAllCalorimetricDataAsync(
            [Parent] Component component,
            CalorimetricDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = CalorimetricDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<CalorimetricDataConnection, CalorimetricDataEdge, CalorimetricData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new CalorimetricDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new CalorimetricDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllCalorimetricDataAsync(
                    database,
                    CalorimetricDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<GeometricDataConnection> GetAllGeometricDataAsync(
            [Parent] Component component,
            GeometricDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = GeometricDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<GeometricDataConnection, GeometricDataEdge, GeometricData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new GeometricDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new GeometricDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllGeometricDataAsync(
                    database,
                    GeometricDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<HygrothermalDataConnection> GetAllHygrothermalDataAsync(
            [Parent] Component component,
            HygrothermalDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = HygrothermalDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<HygrothermalDataConnection, HygrothermalDataEdge, HygrothermalData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new HygrothermalDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new HygrothermalDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllHygrothermalDataAsync(
                    database,
                    HygrothermalDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<LifeCycleDataConnection> GetAllLifeCycleDataAsync(
            [Parent] Component component,
            LifeCycleDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = LifeCycleDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<LifeCycleDataConnection, LifeCycleDataEdge, LifeCycleData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new LifeCycleDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new LifeCycleDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllLifeCycleDataAsync(
                    database,
                    LifeCycleDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<OpticalDataConnection> GetAllOpticalDataAsync(
            [Parent] Component component,
            OpticalDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = OpticalDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<OpticalDataConnection, OpticalDataEdge, OpticalData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new OpticalDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new OpticalDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllOpticalDataAsync(
                    database,
                    OpticalDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        [ListSize(
            AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
            SlicingArguments = ["first", "last"],
            SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
            SizedFields = ["edges", "nodes"],
            RequireOneSlicingArgument = false
        )]
        public Task<PhotovoltaicDataConnection> GetAllPhotovoltaicDataAsync(
            [Parent] Component component,
            PhotovoltaicDataPropositionInput? where,
            [GraphQLType<LocaleType>] string? locale,
            int? first,
            string? after,
            int? last,
            string? before,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        {
            var componentIdPropositionInput = PhotovoltaicDataPropositionInput.Empty with
            {
                ComponentId = DataX.UuidPropositionInput.Empty with
                {
                    EqualTo = component.Id
                }
            };
            return dataQueries.GetAllDataAsync<PhotovoltaicDataConnection, PhotovoltaicDataEdge, PhotovoltaicData>(
                first,
                after,
                last,
                before,
                (edges, totalCount, pageInfo) => new PhotovoltaicDataConnection(edges, totalCount, pageInfo),
                (node, cursor) => new PhotovoltaicDataEdge(cursor, node),
                (database, first, after, last, before) => dataQueries.GetAllPhotovoltaicDataAsync(
                    database,
                    PhotovoltaicDataPropositionInput.Empty with
                    {
                        And = where is null
                            ? [componentIdPropositionInput]
                            : [componentIdPropositionInput, where]
                    },
                    locale,
                    first,
                    after,
                    last,
                    before,
                    resolverContext,
                    cancellationToken
                ),
                cancellationToken
            );
        }

        internal static Task<bool> IsAuthorizedToUpdateNodeAsync(
            [Parent] Component component,
            ClaimsPrincipal claimsPrincipal,
            ComponentAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToUpdate(claimsPrincipal, component.Id, cancellationToken);
        }
    }
}