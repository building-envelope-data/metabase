using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Errors = Infrastructure.Errors;

namespace Infrastructure.Queries
{
    public sealed class GetBackwardOneToManyAssociateOfModelsQuery<TAssociateModel, TAssociationModel, TModel>
      : GetOneToManyAssociatesOfModelsQuery<TAssociateModel, TAssociationModel, TModel>,
        IQuery<IEnumerable<Result<TModel, Errors>>>
    {
        private GetBackwardOneToManyAssociateOfModelsQuery(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
            : base(timestampedIds)
        {
        }

        public static Result<GetBackwardOneToManyAssociateOfModelsQuery<TAssociateModel, TAssociationModel, TModel>, Errors> From(
            IReadOnlyCollection<TimestampedId> timestampedIds
            )
        {
            return Result.Success<GetBackwardOneToManyAssociateOfModelsQuery<TAssociateModel, TAssociationModel, TModel>, Errors>(
                new GetBackwardOneToManyAssociateOfModelsQuery<TAssociateModel, TAssociationModel, TModel>(
                  timestampedIds: timestampedIds
                  )
                );
        }
    }
}