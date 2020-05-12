using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetBackwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        private GetBackwardAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetBackwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetBackwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetBackwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}