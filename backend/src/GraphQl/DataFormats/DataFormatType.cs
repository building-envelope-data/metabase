using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.Users;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl.DataFormats;

public sealed class DataFormatType
    : EntityType<DataFormat, DataFormatByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<DataFormat> descriptor
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
                    context.Parent<DataFormat>()
                )
            );
        descriptor
            .Field(t => t.ManagerId)
            .Ignore();
        descriptor
            .Field("canCurrentUserUpdateNode")
            .ResolveWith<DataFormatResolvers>(x =>
                DataFormatResolvers.CanCurrentUserUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class DataFormatResolvers
    {
        public static Task<bool> CanCurrentUserUpdateNodeAsync(
            [Parent] DataFormat dataFormat,
            ClaimsPrincipal claimsPrincipal,
            DataFormatAuthorization authorization,
            CancellationToken cancellationToken
        )
        {
            return authorization.IsAuthorizedToUpdate(claimsPrincipal, dataFormat.Id, cancellationToken);
        }
    }
}