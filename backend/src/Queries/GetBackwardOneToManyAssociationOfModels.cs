using System.Collections.Generic;
using CSharpFunctionalExtensions;
using ValueObjects = Icon.ValueObjects;
using Icon.Infrastructure.Query;

namespace Icon.Queries
{
    public sealed class GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>
      : GetOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel>,
        IQuery<IEnumerable<Result<TAssociationModel, Errors>>>
    {
        private GetBackwardOneToManyAssociationOfModels(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<ValueObjects.TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>, Errors>(
                new GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}