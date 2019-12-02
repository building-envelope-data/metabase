using System;
using Icon.Infrastructure.Query;
using Icon.Infrastructure.Command;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Commands = Icon.Commands;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;
using QueryException = HotChocolate.Execution.QueryException;

namespace Icon.GraphQl
{
    public sealed class Mutation
      : QueryAndMutationBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<Models.User> _userManager;

        public Mutation(ICommandBus commandBus, IQueryBus queryBus, UserManager<Models.User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }

        public async Task<Component> CreateComponent(
            ComponentInput input,
            IResolverContext resolverContext
            )
        {
            var validatedInput =
              ResultHelpers.HandleFailure(
                  input.Validate(path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  );
            var command =
              ResultHelpers.HandleFailure(
                  Commands.CreateComponent.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
                    )
                  );
            var result =
              await _commandBus
                .Send<
                   Commands.CreateComponent,
                   Result<ValueObjects.TimestampedId, Errors>
                 >(command);
            var timestampedId = ResultHelpers.HandleFailure(result);
            Timestamp.Store(timestampedId.Timestamp, resolverContext);
            var query =
              ResultHelpers.HandleFailure(
                Queries.GetComponent.From(timestampedId)
                );
            return
              Component.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponent,
                         Result<Models.Component, Errors>
                         >(query)
                    ),
                  timestampedId.Timestamp
                  );
        }

        public async Task<ComponentVersion> CreateComponentVersion(
            ComponentVersionInput input,
            IResolverContext resolverContext
            )
        {
            var validatedInput =
              ResultHelpers.HandleFailure(
                  input.Validate(path: Array.Empty<object>()) // TODO What is the proper path for variables?
                  );
            var command =
              ResultHelpers.HandleFailure(
                  Commands.CreateComponentVersion.From(
                    input: validatedInput,
                    creatorId: ValueObjects.Id.From(Guid.NewGuid()).Value // TODO Use current user!
                    )
                  );
            var result =
              await _commandBus
               .Send<
                  Commands.CreateComponentVersion,
                  Result<ValueObjects.TimestampedId, Errors>
                >(command);
            var timestampedId = ResultHelpers.HandleFailure(result);
            Timestamp.Store(timestampedId.Timestamp, resolverContext);
            var query =
              ResultHelpers.HandleFailure(
                Queries.GetComponentVersion.From(timestampedId)
                );
            return
              ComponentVersion.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponentVersion,
                         Result<Models.ComponentVersion, Errors>
                         >(query)
                    ),
                  timestampedId.Timestamp
                  );
        }
    }
}