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
            var command =
              new Commands.CreateComponent(
                  information: new Models.ComponentInformation(
                    name: input.Name,
                    abbreviation: input.Abbreviation,
                    description: input.Description,
                    availableFrom: input.AvailableFrom,
                    availableUntil: input.AvailableUntil,
                    categories: input.Categories
                    ),
                    creatorId: Guid.NewGuid() // TODO Use current user!
                  );
            var result =
              await _commandBus
                .Send<
                   Commands.CreateComponent,
                   Result<(Guid Id, DateTime Timestamp), IError>
                 >(command);
            var (id, timestamp) = ResultHelpers.HandleFailure(result);
            Timestamp.Store(timestamp, resolverContext);
            return
              Component.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponent,
                         Result<Models.Component, IError>
                         >(new Queries.GetComponent(id, timestamp))
                    ),
                  timestamp
                  );
        }

        public async Task<ComponentVersion> CreateComponentVersion(
            ComponentVersionInput input,
            IResolverContext resolverContext
            )
        {
            var command =
              new Commands.CreateComponentVersion(
                  input.ComponentId,
                  information: new Models.ComponentInformation(
                    name: input.Name,
                    abbreviation: input.Abbreviation,
                    description: input.Description,
                    availableFrom: input.AvailableFrom,
                    availableUntil: input.AvailableUntil,
                    categories: input.Categories
                    ),
                    creatorId: Guid.NewGuid() // TODO Use current user!
                  );
            var result =
              await _commandBus
               .Send<
                  Commands.CreateComponentVersion,
                  Result<(Guid Id, DateTime Timestamp), IError>
                >(command);
            var (id, timestamp) = ResultHelpers.HandleFailure(result);
            Timestamp.Store(timestamp, resolverContext);
            return
              ComponentVersion.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponentVersion,
                         Result<Models.ComponentVersion, IError>
                         >(new Queries.GetComponentVersion(id, timestamp))
                    ),
                  timestamp
                  );
        }
    }
}