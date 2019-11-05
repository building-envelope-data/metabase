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

        public async Task<Component> CreateComponent()
        {
          return
            Component.FromModel(
                (await _commandBus
                 .Send<
                    Commands.CreateComponent,
                    Models.Component
                    >(new Commands.CreateComponent(creatorId: Guid.NewGuid()))) // TODO Use current user!
                );
        }

        public async Task<ComponentVersion> CreateComponentVersion(ComponentVersionInput componentVersionInput)
        {
          return
            ComponentVersion.FromModel(
                (await _commandBus
                 .Send<
                    Commands.CreateComponentVersion,
                    Models.ComponentVersion
                    >(new Commands.CreateComponentVersion(
                        componentVersionInput.ComponentId,
                        creatorId: Guid.NewGuid()) // TODO Use current user!
                     ))
                );
        }
    }
}