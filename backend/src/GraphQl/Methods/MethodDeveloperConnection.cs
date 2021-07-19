using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;

namespace Metabase.GraphQl.Methods
{
    public sealed class MethodDeveloperConnection
    {
        private readonly Data.Method _subject;
        private readonly bool _pending;

        public MethodDeveloperConnection(
            Data.Method subject,
            bool pending
            )
        {
            _subject = subject;
            _pending = pending;
        }

        public async Task<IEnumerable<MethodDeveloperEdge>> GetEdgesAsync(
            [DataLoader] InstitutionMethodDevelopersByMethodIdDataLoader institutionMethodDevelopersDataLoader,
            [DataLoader] UserMethodDevelopersByMethodIdDataLoader userMethodDevelopersDataLoader,
            [DataLoader] PendingInstitutionMethodDevelopersByMethodIdDataLoader pendingInstitutionMethodDevelopersDataLoader,
            [DataLoader] PendingUserMethodDevelopersByMethodIdDataLoader pendingUserMethodDevelopersDataLoader,
            CancellationToken cancellationToken
            )
        {
            return
                (await new InstitutionMethodDeveloperConnection(_subject, _pending)
                .GetEdgesAsync(
                    pendingInstitutionMethodDevelopersDataLoader,
                    institutionMethodDevelopersDataLoader,
                    cancellationToken
                    )
                .ConfigureAwait(false)
                )
                .Select(e => new MethodDeveloperEdge(e))
                .Concat(
                (await new UserMethodDeveloperConnection(_subject, _pending)
                .GetEdgesAsync(
                    pendingUserMethodDevelopersDataLoader,
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
        : ForkingConnection<Data.Method, Data.InstitutionMethodDeveloper, PendingInstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDevelopersByMethodIdDataLoader, InstitutionMethodDeveloperEdge>
    {
        public InstitutionMethodDeveloperConnection(
            Data.Method subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new InstitutionMethodDeveloperEdge(x)
                )
        {
        }
    }

    internal sealed class UserMethodDeveloperConnection
        : ForkingConnection<Data.Method, Data.UserMethodDeveloper, PendingUserMethodDevelopersByMethodIdDataLoader, UserMethodDevelopersByMethodIdDataLoader, UserMethodDeveloperEdge>
    {
        public UserMethodDeveloperConnection(
            Data.Method subject,
            bool pending
        )
            : base(
                subject,
                pending,
                x => new UserMethodDeveloperEdge(x)
                )
        {
        }
    }
}