using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>
      : GetOneToManyAssociationsOfModels<TModel, TAssociationModel>,
        IQuery<IEnumerable<Result<IEnumerable<Result<TAssociationModel, Errors>>, Errors>>>
    {
        private GetForwardOneToManyAssociationsOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Ok<GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>, Errors>(
                new GetForwardOneToManyAssociationsOfModels<TModel, TAssociationModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}