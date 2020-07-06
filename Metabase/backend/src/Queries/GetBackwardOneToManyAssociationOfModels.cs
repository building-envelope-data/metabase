using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>
      : GetOneToManyAssociationsOfModels<TAssociateModel, TAssociationModel>,
        IQuery<IEnumerable<Result<TAssociationModel, Errors>>>
    {
        private GetBackwardOneToManyAssociationOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociationOfModels<TAssociateModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
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