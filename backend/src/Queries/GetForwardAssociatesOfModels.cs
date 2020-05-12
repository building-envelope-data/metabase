using ValueObjects = Icon.ValueObjects;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Icon.Queries
{
    public sealed class GetForwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        private GetForwardAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
            : base(timestampedModelIds)
        {
        }

        public static Result<GetForwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedModelIds
            )
        {
            return Result.Ok<GetForwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedModelIds: timestampedModelIds
                  )
                );
        }
    }
}