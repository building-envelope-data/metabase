using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.Entities;
using Metabase.GraphQl.References;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.DataFormats;

public sealed class DataFormatType
    : EntityType<DataFormat, IDataFormatByIdDataLoader>
{
    protected override void Configure(
        IObjectTypeDescriptor<DataFormat> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor
            .Field(t => t.Reference)
            .Type<ReferenceType>()
            .Cost(0)
            .Resolve(context => context
                .Parent<DataFormat>()
                .Reference?
                .TheReference
            );
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
            .Field("isAuthorizedToUpdateNode")
            .Cost(1)
            .ResolveWith<DataFormatResolvers>(x =>
                DataFormatResolvers.IsAuthorizedToUpdateNodeAsync(default!, default!, default!, default!))
            .UseUserManager();
    }

    private sealed class DataFormatResolvers
    {
        public static Task<bool> IsAuthorizedToUpdateNodeAsync(
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