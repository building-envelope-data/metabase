using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Metabase.Queries
{
    public abstract class GetAssociationsOfModels<TModel, TAssociationModel>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected GetAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}