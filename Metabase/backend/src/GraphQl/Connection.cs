using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Metabase.GraphQl
{
    public abstract class Connection<TSubject, TAssociation, TAssociationsByAssociateIdDataLoader, TEdge>
        where TSubject : Infrastructure.Data.IEntity
        where TAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        protected TSubject Subject { get; }
        private readonly Func<TAssociation, TEdge> _createEdge;

        protected Connection(
            TSubject subject,
            Func<TAssociation, TEdge> createEdge
            )
        {
            Subject = subject;
            _createEdge = createEdge;
        }

        public async Task<IEnumerable<TEdge>> GetEdgesAsync(
            [DataLoader] TAssociationsByAssociateIdDataLoader dataLoader,
            CancellationToken cancellationToken
            )
        {
            return (
                await dataLoader.LoadAsync(Subject.Id, cancellationToken)
                .ConfigureAwait(false)
                )
                .Select(_createEdge);
        }
    }
}