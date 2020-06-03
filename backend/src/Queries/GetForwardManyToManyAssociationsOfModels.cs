using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TModel, TAssociationModel>
    {
        private GetForwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardManyToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}