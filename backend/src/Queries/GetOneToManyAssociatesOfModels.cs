using System.Collections.Generic;

namespace Icon.Queries
{
    public abstract class GetOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetOneToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}