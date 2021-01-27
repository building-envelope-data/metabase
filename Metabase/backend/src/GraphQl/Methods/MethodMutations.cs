using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Metabase.GraphQl.Methods
{
    [ExtendObjectType(Name = nameof(Mutation))]
    public sealed class MethodMutations
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [Authorize]
        public async Task<CreateMethodPayload> CreateMethodAsync(
            CreateMethodInput input,
            [ScopedService] Data.ApplicationDbContext context,
            CancellationToken cancellationToken
            )
        {
            var method = new Data.Method(
                name: input.Name,
                description: input.Description,
                publicationLocator: input.PublicationLocator,
                codeLocator: input.CodeLocator,
                categories: input.Categories
            );
            context.Methods.Add(method);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return new CreateMethodPayload(method);
        }
    }
}