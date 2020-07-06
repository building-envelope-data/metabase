using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Metabase.Queries
{
    public abstract class GetOneToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>
    {
        protected GetOneToManyAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}