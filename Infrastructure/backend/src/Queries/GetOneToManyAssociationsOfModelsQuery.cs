using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public abstract class GetOneToManyAssociationsOfModelsQuery<TModel, TAssociationModel>
      : GetAssociationsOfModelsQuery<TModel, TAssociationModel>
    {
        protected GetOneToManyAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}