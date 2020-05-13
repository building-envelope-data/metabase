using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetForwardAssociationsOfModels<TModel, TAssociationModel>
      : GetAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetForwardAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetForwardAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetForwardAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}