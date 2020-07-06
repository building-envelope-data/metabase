using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>
      : GetManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>
    {
        private GetBackwardManyToManyAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>, Errors>(
                new GetBackwardManyToManyAssociationsOfModels<TAssociateModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}