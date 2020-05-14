using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetForwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}