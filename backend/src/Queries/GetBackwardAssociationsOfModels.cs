using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetBackwardAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetBackwardAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetBackwardAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetBackwardAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}