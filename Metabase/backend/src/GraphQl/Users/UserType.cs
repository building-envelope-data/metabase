using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Users
{
    public sealed class UserType
      : ObjectType<Data.User>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.User> descriptor
            )
        {
            descriptor.BindFieldsExplicitly();
            descriptor
                .ImplementsNode()
                .IdField(t => t.Id)
                .ResolveNode((context, id) =>
                    context
                    .DataLoader<GraphQl.Users.UserByIdDataLoader>()
                    .LoadAsync(id, context.RequestAborted)
                    );

            descriptor
              .Field(t => t.Email);

            /* descriptor */
            /*     .Field(t => t.SessionsUsers) */
            /*     .ResolveWith<UserResolvers>(t => t.GetSessionsAsync(default!, default!, default!, default)) */
            /*     .UseDbContext<ApplicationDbContext>() */
            /*     .Name("sessions"); */
        }

        /* private class UserResolvers */
        /* { */
        /*     public async Task<IEnumerable<Session>> GetSessionsAsync( */
        /*         Data.User user, */
        /*         [ScopedService] ApplicationDbContext dbContext, */
        /*         SessionByIdDataLoader sessionById, */
        /*         CancellationToken cancellationToken) */
        /*     { */
        /*         int[] speakerIds = await dbContext.Users */
        /*             .Where(a => a.Id == user.Id) */
        /*             .Include(a => a.SessionsUsers) */
        /*             .SelectMany(a => a.SessionsUsers.Select(t => t.SessionId)) */
        /*             .ToArrayAsync(); */

        /*         return await sessionById.LoadAsync(speakerIds, cancellationToken); */
        /*     } */
        /* } */
    }
}