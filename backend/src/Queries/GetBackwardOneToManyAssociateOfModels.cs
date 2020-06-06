using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public sealed class GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>
      : GetOneToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<TModel, Errors>>>
    {
        private GetBackwardOneToManyAssociateOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>, Errors>(
                new GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}
