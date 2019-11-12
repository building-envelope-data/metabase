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
            IResolverContext context
            )
        {
            var result =
              await _commandBus
                .Send<
                   Commands.CreateComponent,
                   Result<(Guid Id, DateTime Timestamp), IError>
                 >(new Commands.CreateComponent(creatorId: Guid.NewGuid())); // TODO Use current user!
            var (id, timestamp) = ResultHelpers.HandleFailure(result);
            return
              Component.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponent,
                         Result<Models.Component, IError>
                         >(new Queries.GetComponent(id, timestamp))
                    )
                  );
        }

        public async Task<ComponentVersion> CreateComponentVersion(
            ComponentVersionInput componentVersionInput,
            IResolverContext context
            )
        {
            var result =
              await _commandBus
               .Send<
                  Commands.CreateComponentVersion,
                  Result<(Guid Id, DateTime Timestamp), IError>
                >(new Commands.CreateComponentVersion(
                    componentVersionInput.ComponentId,
                    creatorId: Guid.NewGuid()
                    )
                    ); // TODO Use current user!
            var (id, timestamp) = ResultHelpers.HandleFailure(result);
            return
              ComponentVersion.FromModel(
                  ResultHelpers.HandleFailure(
                    await _queryBus
                      .Send<
                         Queries.GetComponentVersion,
                         Result<Models.ComponentVersion, IError>
                         >(new Queries.GetComponentVersion(id, timestamp))
                    )
                  );
        }

        /* lastRead */
    }
}