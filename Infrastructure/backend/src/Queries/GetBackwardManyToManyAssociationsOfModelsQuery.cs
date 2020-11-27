using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;

namespace Infrastructure.Queries
{
    public sealed class GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>
      : GetManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>
    {
        private GetBackwardManyToManyAssociationsOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>, Errors>(
                new GetBackwardManyToManyAssociationsOfModelsQuery<TAssociateModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}