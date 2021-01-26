using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore.Authorization;
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.GraphQl.Institutions
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class InstitutionMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize]
        public async Task<CreateInstitutionPayload> CreateInstitutionAsync(
            CreateInstitutionInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var institution = new Data.Institution(
                name: input.Name,
                abbreviation: input.Abbreviation,
                description: input.Description,
                websiteLocator: input.WebsiteLocator,
                publicKey: input.PublicKey,
                state: input.State
            );
            context.Institutions.Add(institution);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateInstitutionPayload(institution);
        }
    }
}
