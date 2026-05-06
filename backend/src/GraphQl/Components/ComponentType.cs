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
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.Users;

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
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<ComponentResolvers>(x =>
                ComponentResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class ComponentResolvers
    {
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