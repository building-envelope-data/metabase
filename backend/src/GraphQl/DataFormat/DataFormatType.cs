using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.DataFormats
{
    public sealed class DataFormatType
      : EntityType<Data.DataFormat, DataFormatByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.DataFormat> descriptor
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
                .Type<NonNullType<ObjectType<DataFormatManagerEdge>>>()
                .Resolve(context =>
                    new DataFormatManagerEdge(
                        context.Parent<Data.DataFormat>()
                        )
                    );
            descriptor
                .Field(t => t.ManagerId)
                .Ignore();
            descriptor
              .Field("canCurrentUserUpdateNode")
              .ResolveWith<DataFormatResolvers>(x => x.GetCanCurrentUserUpdateNodeAsync(default!, default!, default!, default!, default!))
              .UseUserManager();
        }

        private sealed class DataFormatResolvers
        {
                public Task<bool> GetCanCurrentUserUpdateNodeAsync(
                  [Parent] Data.DataFormat dataFormat,
                  ClaimsPrincipal claimsPrincipal,
                  [Service(ServiceKind.Resolver)] UserManager<Data.User> userManager,
                  Data.ApplicationDbContext context,
                  CancellationToken cancellationToken
                )
                {
                    return DataFormatAuthorization.IsAuthorizedToUpdate(claimsPrincipal, dataFormat.Id, userManager, context, cancellationToken);
                }
        }
    }
}