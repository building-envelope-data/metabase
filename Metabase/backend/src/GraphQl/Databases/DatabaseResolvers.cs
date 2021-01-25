using System.Threading;
using System.Threading.Tasks;
using Metabase.GraphQl.Institutions;

namespace Metabase.GraphQl.Databases
{
    public class DatabaseResolvers
    {
        public async Task<Data.Institution> GetOperatorAsync(
            Data.Database database,
            InstitutionByIdDataLoader institutionById,
            CancellationToken cancellationToken
            )
        {
            return (
                await institutionById.LoadAsync(
                database.OperatorId,
                 cancellationToken
                 ).ConfigureAwait(false)
                 )!;
        }
    }
}