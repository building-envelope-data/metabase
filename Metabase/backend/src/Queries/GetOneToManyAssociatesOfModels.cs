using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Metabase.Queries
{
    public abstract class GetOneToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        protected GetOneToManyAssociatesOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}