using Icon.Infrastructure.Command;
using Icon.Infrastructure.Query;
using Microsoft.AspNetCore.Identity;
using User = Icon.Models.User;

namespace Icon.GraphQl
{
    public sealed class Mutation
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly UserManager<User> _userManager;

        public Mutation(ICommandBus commandBus, IQueryBus queryBus, UserManager<User> userManager)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _userManager = userManager;
        }
    }
}