using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;

namespace Metabase.GraphQl
{
    public abstract class ForkingConnection<TSubject, TAssociation, TSomeAssociationsByAssociateIdDataLoader, TOtherAssociationsByAssociateIdDataLoader, TEdge>
        where TSubject : Data.IEntity
        where TSomeAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
        where TOtherAssociationsByAssociateIdDataLoader : IDataLoader<Guid, TAssociation[]>
    {
        protected TSubject Subject { get; }
        private readonly bool _useFirstDataLoader;
        private readonly Func<TAssociation, TEdge> _createEdge;

        protected ForkingConnection(
            TSubject subject,
            bool useFirstDataLoader,
            Func<TAssociation, TEdge> createEdge
            )
        {
            Subject = subject;
            _useFirstDataLoader = useFirstDataLoader;
            _createEdge = createEdge;
        }

        public Task<IEnumerable<TEdge>> GetEdgesAsync(
            [DataLoader] TSomeAssociationsByAssociateIdDataLoader someDataLoader,
            [DataLoader] TOtherAssociationsByAssociateIdDataLoader otherDataLoader,
            CancellationToken cancellationToken
            )
        {
            return _useFirstDataLoader
                ? GetEdgesAsync(someDataLoader, cancellationToken)
                : GetEdgesAsync(otherDataLoader, cancellationToken);
        }

        private async Task<IEnumerable<TEdge>> GetEdgesAsync<TDataLoader>(
            TDataLoader dataLoader,
            CancellationToken cancellationToken
            )
            where TDataLoader : IDataLoader<Guid, TAssociation[]>
        {
            return (
                await dataLoader.LoadAsync(Subject.Id, cancellationToken)
                .ConfigureAwait(false)
                )
                .Select(_createEdge);
        }
    }
}