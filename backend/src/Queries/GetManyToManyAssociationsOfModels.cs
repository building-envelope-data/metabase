using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public abstract class GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        protected GetManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
          : base(timestampedIds)
        {
        }
    }
}