using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetAssociationsOfModelsQuery<TModel, TAssociationModel>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected GetAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}