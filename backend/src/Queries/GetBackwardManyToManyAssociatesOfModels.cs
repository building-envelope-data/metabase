using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        private GetBackwardManyToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetBackwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}