using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl.Users;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodType
        : EntityType<Data.Method, MethodByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.Method> descriptor
        )
        {
            base.Configure(descriptor);
            descriptor
                .Field(t => t.Standard)
                .Ignore();
            descriptor
                .Field(t => t.Publication)
                .Ignore();
            descriptor
                .Field(t => t.Manager)
                .Type<NonNullType<ObjectType<MethodManagerEdge>>>()
                .Resolve(context =>
                    new MethodManagerEdge(
                        context.Parent<Data.Method>()
                    )
                );
            descriptor
                .Field(t => t.ManagerId)
                .Ignore();
            descriptor
                .Field(t => t.Developers)
                .Argument(nameof(Data.IMethodDeveloper.Pending).FirstCharToLower(),
                    _ => _.Type<NonNullType<BooleanType>>().DefaultValue(false))
                .Type<NonNullType<ObjectType<MethodDeveloperConnection>>>()
                .Resolve(context =>
                    new MethodDeveloperConnection(
                        context.Parent<Data.Method>(),
                        context.ArgumentValue<bool?>(nameof(Data.IMethodDeveloper.Pending).FirstCharToLower()) ?? false
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
                .Field("canCurrentUserUpdateNode")
                .ResolveWith<MethodResolvers>(x =>
                    MethodResolvers.GetCanCurrentUserUpdateNodeAsync(default!, default!, default!, default!, default!))
                .UseUserManager();
        }

        private sealed class MethodResolvers
        {
            public static Task<bool> GetCanCurrentUserUpdateNodeAsync(
                [Parent] Data.Method method,
                ClaimsPrincipal claimsPrincipal,
                [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
                Data.ApplicationDbContext context,
                CancellationToken cancellationToken
            )
            {
                return MethodAuthorization.IsAuthorizedToUpdate(claimsPrincipal, method.Id, userManager, context,
                    cancellationToken);
            }
        }
    }
}