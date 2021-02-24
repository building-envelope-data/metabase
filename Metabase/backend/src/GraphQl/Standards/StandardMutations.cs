using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace Metabase.GraphQl.Standards
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class StandardMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize]
        public async Task<CreateStandardPayload> CreateStandardAsync(
            CreateStandardInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var standard = new Data.Standard(
                title: input.Title,
                @abstract: input.Abstract,
                section: input.Section,
                year: input.Year,
                // TODO numeration: input.Numeration,
                standardizers: input.Standardizers,
                locator: input.Locator
            );
            context.Standards.Add(standard);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateStandardPayload(standard);
        }
    }
}