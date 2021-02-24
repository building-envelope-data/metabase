using HotChocolate.Types;

namespace Metabase.GraphQl.Users
{
    public sealed class UserType
      : EntityType<Data.User, UserByIdDataLoader>
    {
        protected override void Configure(
            IObjectTypeDescriptor<Data.User> descriptor
            )
        {
            descriptor.BindFieldsExplicitly();
            base.Configure(descriptor);
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