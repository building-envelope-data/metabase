using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Extensions;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Extensions;
using Metabase.GraphQl.References;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods;

public sealed class MethodType
    : EntityType<Method, IMethodByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.Reference)
            .Type<ReferenceType>()
            .Cost(0)
            .Resolve(context => context
                .Parent<Method>()
                .Reference?
                .TheReference
            );
        descriptor
            .Field(t => t.Manager)
            .Type<NonNullType<ObjectType<MethodManagerEdge>>>()
            .Resolve(context =>
                new MethodManagerEdge(
                    context.Parent<Method>()
                )
            );
        descriptor
            .Field(t => t.ManagerId)
            .Ignore();
        descriptor
            .Field(t => t.Developers)
            .Type<NonNullType<ObjectType<MethodDeveloperConnection>>>()
            .UseFiltering<MethodDeveloperFilterType>()
            .UseSorting<MethodDeveloperSortType>()
            .Resolve(context =>
                new MethodDeveloperConnection(
                    context.Parent<Method>(),
                    context.GetQueryContext<IMethodDeveloper>()
                )
            );
        descriptor
            .Field($"{GraphQlConstants.PendingPrefix}{nameof(Method.Developers)}")
            .Type<ObjectType<PendingMethodDeveloperConnection>>()
            .Authorize(AuthorizationPolicies.WriteScopePolicy)
            .UseFiltering<MethodDeveloperFilterType>()
            .UseSorting<MethodDeveloperSortType>()
            .Resolve(context =>
                new PendingMethodDeveloperConnection(
                    context.Parent<Method>(),
                    context.GetQueryContext<IMethodDeveloper>()
                )
            );
        descriptor
            .Field(t => t.InstitutionDevelopers)
            .Ignore();
        descriptor
            .Field(t => t.InstitutionDeveloperEdges)
            .Ignore();
        descriptor
            .Field(t => t.UserDevelopers)
            .Ignore();
        descriptor
            .Field(t => t.UserDeveloperEdges)
            .Ignore();
        descriptor
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<MethodResolvers>(x =>
                MethodResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class MethodResolvers
    {
        public static Task<bool> IsAuthorizedToUpdateNodeAsync(
            [Parent] Method method,
            ClaimsPrincipal claimsPrincipal,
            MethodAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToUpdate(claimsPrincipal, method.Id, cancellationToken);
        }
    }
}