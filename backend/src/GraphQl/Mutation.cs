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
            var (id, timestamp) =
              await _commandBus
                .Send<
                   Commands.CreateComponent,
                   (Guid Id, DateTime Timestamp)
                 >(new Commands.CreateComponent(creatorId: Guid.NewGuid())); // TODO Use current user!
            return
              Component.FromModel(
                  await _queryBus
                    .Send<
                       Queries.GetComponent,
                       Models.Component
                       >(new Queries.GetComponent(id, timestamp))
                  );
        }

        public async Task<ComponentVersion> CreateComponentVersion(
            ComponentVersionInput componentVersionInput,
            IResolverContext context
            )
        {
            var (id, timestamp) =
              await _commandBus
               .Send<
                  Commands.CreateComponentVersion,
                  (Guid Id, DateTime Timestamp)
                >(new Commands.CreateComponentVersion(
                    componentVersionInput.ComponentId,
                    creatorId: Guid.NewGuid()
                    )
                    ); // TODO Use current user!
            return
              ComponentVersion.FromModel(
                  await _queryBus
                    .Send<
                       Queries.GetComponentVersion,
                       Models.ComponentVersion
                       >(new Queries.GetComponentVersion(id, timestamp))
                  );
        }

        /* lastRead */
    }
}