using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Metabase.Queries
{
    public abstract class GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        public IReadOnlyCollection<TimestampedId> TimestampedIds { get; }

        protected GetAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            TimestampedIds = timestampedIds;
        }
    }
}