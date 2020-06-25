using System.Collections.Generic;

namespace Icon.Queries
{
    public abstract class GetAssociationsOfModels<TModel, TAssociationModel>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        protected GetAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}