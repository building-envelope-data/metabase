using System.Collections.Generic;

namespace Icon.Queries
{
    public abstract class GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        public IReadOnlyCollection<ValueObjects.TimestampedId> TimestampedIds { get; }

        protected GetAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}