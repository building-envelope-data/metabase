using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetAssociatesOfModelsQuery<TModel, TAssociationModel, TAssociateModel>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected GetAssociatesOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}