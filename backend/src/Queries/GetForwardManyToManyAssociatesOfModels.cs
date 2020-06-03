using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Queries
{
    public sealed class GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
      : GetManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>
    {
        private GetForwardManyToManyAssociatesOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>, Errors>(
                new GetForwardManyToManyAssociatesOfModels<TModel, TAssociationModel, TAssociateModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}