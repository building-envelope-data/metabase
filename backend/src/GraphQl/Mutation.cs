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
            var componentId =
              await _commandBus
                .Send<
                   Commands.CreateComponent,
                   Guid
                 >(new Commands.CreateComponent(creatorId: Guid.NewGuid())); // TODO Use current user!
            var timestamp = SetTimestamp(DateTime.UtcNow, context);
            return
              Component.FromModel(
                  await _queryBus
                    .Send<
                       Queries.GetComponent,
                       Models.Component
                       >(new Queries.GetComponent(componentId, timestamp))
                  );
        }

        public async Task<ComponentVersion> CreateComponentVersion(
            ComponentVersionInput componentVersionInput,
            IResolverContext context
            )
        {
            var versionId =
              await _commandBus
               .Send<
                  Commands.CreateComponentVersion,
                  Guid
                >(new Commands.CreateComponentVersion(
                    componentVersionInput.ComponentId,
                    creatorId: Guid.NewGuid()
                    )
                    ); // TODO Use current user!
            var timestamp = SetTimestamp(DateTime.UtcNow, context);
            return
              ComponentVersion.FromModel(
                  await _queryBus
                    .Send<
                       Queries.GetComponentVersion,
                       Models.ComponentVersion
                       >(new Queries.GetComponentVersion(versionId, timestamp))
                  );
        }

        /* lastRead */
    }
}