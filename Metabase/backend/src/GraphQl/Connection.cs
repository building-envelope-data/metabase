using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using System.Linq;

namespace Metabase.GraphQl
{
    public abstract class Connection<TSubject, TAssociation, TAssociationsByAssociateIdDataLoader, TEdge>
        where TSubject : Infrastructure.Data.IEntity
        where TAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        private readonly TSubject _subject;
        private readonly Func<TAssociation, TEdge> _createEdge;

        protected Connection(
            TSubject subject,
            Func<TAssociation, TEdge> createEdge
            )
        {
            _subject = subject;
            _createEdge = createEdge;
        }

        public async Task<IEnumerable<TEdge>> GetEdges(
            [DataLoader] TAssociationsByAssociateIdDataLoader dataLoader,
            CancellationToken cancellationToken
            )
        {
            return (
                await dataLoader.LoadAsync(_subject.Id, cancellationToken)
                .ConfigureAwait(false)
                )
                .Select(_createEdge);
        }
    }
}