using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;

namespace Metabase.GraphQl.Databases
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class DatabaseMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize]
        public async Task<CreateDatabasePayload> CreateDatabaseAsync(
            CreateDatabaseInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var database = new Data.Database(
                name: input.Name,
                description: input.Description,
                locator: input.Locator
            );
            context.Databases.Add(database);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateDatabasePayload(database);
        }
    }
}
