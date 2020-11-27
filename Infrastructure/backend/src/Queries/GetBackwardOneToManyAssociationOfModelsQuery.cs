using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>
      : GetOneToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>,
        IQuery<IEnumerable<Result<TAssociationModel, Errors>>>
    {
        private GetBackwardOneToManyAssociationOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>, Errors>(
                new GetBackwardOneToManyAssociationOfModelsQuery<TAssociateModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}