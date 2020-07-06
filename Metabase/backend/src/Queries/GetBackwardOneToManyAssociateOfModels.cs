using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Metabase.Queries
{
    public sealed class GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>
      : GetOneToManyAssociatesOfModels<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<TModel, Errors>>>
    {
        private GetBackwardOneToManyAssociateOfModels(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociateOfModels<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
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