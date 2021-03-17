using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using Metabase.GraphQl.Institutions;
using Metabase.GraphQl.Users;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodDeveloperConnection
    {
        private readonly Data.Method _subject;
        public MethodDeveloperConnection(
            Data.Method subject
            )
        {
            _subject = subject;
        }

        public async Task<IEnumerable<MethodDeveloperEdge>> GetEdgesAsync(
            [DataLoader] InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
            [DataLoader] UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
            CancellationToken cancellationToken
            )
        {
            return
                (await new InstitutionMethodDeveloperConnection(_subject)
                .GetEdgesAsync(
                    institutionMethodDevelopersDataLoader,
                    cancellationToken
                    )
                .ConfigureAwait(false)
                )
                .Select(e => new MethodDeveloperEdge(e))
                .Concat(
                (await new UserMethodDeveloperConnection(_subject)
                .GetEdgesAsync(
                    userMethodDevelopersDataLoader,
                    cancellationToken
                    )
                .ConfigureAwait(false)
                )
                .Select(e => new MethodDeveloperEdge(e))
                );
        }
    }

    internal sealed class InstitutionMethodDeveloperConnection
        : Connection<Data.Method, Data.InstitutionMethodDeveloper, InstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDeveloperEdge>
    {
        public InstitutionMethodDeveloperConnection(
            Data.Method subject
        )
            : base(
                subject,
                x => new InstitutionMethodDeveloperEdge(x)
                )
        {
        }
    }

    internal sealed class UserMethodDeveloperConnection
        : Connection<Data.Method, Data.UserMethodDeveloper, UserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperEdge>
    {
        public UserMethodDeveloperConnection(
            Data.Method subject
        )
            : base(
                subject,
                x => new UserMethodDeveloperEdge(x)
                )
        {
        }
    }
}