using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.Databases
{
    [ExtendObjectType(Name = nameof(Query))]
    public sealed class DatabaseQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Data.Database> GetDatabases(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return context.Databases;
        }

        public Task<Data.Database?> GetDatabaseAsync(
            Guid uuid,
            DatabaseByIdDataLoader databaseById,
            CancellationToken cancellationToken
            )
        {
            return databaseById.LoadAsync(
                uuid,
                cancellationToken
                );
        }
    }
}