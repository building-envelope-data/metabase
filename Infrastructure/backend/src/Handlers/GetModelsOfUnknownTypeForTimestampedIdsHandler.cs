using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CancellationToken = System.Threading.CancellationToken;
using ErrorCodes = Infrastructure.ErrorCodes;
using Errors = Infrastructure.Errors;
using Exception = System.Exception;
using Type = System.Type;

namespace Infrastructure.Handlers
{
    public abstract class GetModelsOfUnknownTypeForTimestampedIdsHandler
      : Queries.IQueryHandler<Queries.GetModelsForTimestampedIds<Models.IModel>, IEnumerable<Result<Models.IModel, Errors>>>
    {
        private readonly Models.IModelRepository _repository;
        private readonly IDictionary<Type, IGetModelsForTimestampedIdsHandler> _aggregateTypeToGetHandler;

        protected GetModelsOfUnknownTypeForTimestampedIdsHandler(
            Models.IModelRepository repository,
            IDictionary<Type, IGetModelsForTimestampedIdsHandler> aggregateTypeToGetHandler
            )
        {
            _repository = repository;
            _aggregateTypeToGetHandler = aggregateTypeToGetHandler;
        }

        public async Task<IEnumerable<Result<Models.IModel, Errors>>> Handle(
            Queries.GetModelsForTimestampedIds<Models.IModel> query,
            CancellationToken cancellationToken
            )
        {
            using (var session = _repository.OpenReadOnlySession())
            {
                return await session.LoadAllX<Models.IModel>(
                    query.TimestampedIds,
                    (session, aggregateType, timestampedIds, cancellationToken) =>
                    {
                        if (_aggregateTypeToGetHandler.ContainsKey(aggregateType))
                        {
                            return _aggregateTypeToGetHandler[aggregateType].HandleX(
                                session,
                                timestampedIds,
                                cancellationToken
                              );
                        }
                        // TODO Return failure result instead.
                        throw new Exception($"The aggregate type {aggregateType} is not supported");
                        /* return Result.Failure<Models.MethodDeveloper, Errors>( */
                        /*     Errors.One( */
                        /*       message: $"The aggregate type {aggregateType} is invalid", */
                        /*       code: ErrorCodes.InvalidType */
                        /*       ) */
                        /*     ); */
                    },
                    cancellationToken
                    )
                    .ConfigureAwait(false);
            }
        }
    }
}