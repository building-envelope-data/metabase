using System.Collections.Generic;

namespace Icon.Queries
{
    public abstract class GetOneToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>
    {
        protected GetOneToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}