using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using User = Icon.Models.User;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;

namespace Icon.GraphQl
{
    public sealed class Query
    {
        private readonly IQueryBus _queryBus;
        private readonly UserManager<User> _userManager;

        public Query(IQueryBus queryBus, UserManager<User> userManager)
        {
            _queryBus = queryBus;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Component>> GetComponents()
        {
            return (await _queryBus.Send<Queries.ListComponents, IEnumerable<Models.Component>>(new Queries.ListComponents()))
              .Select(c => Component.FromModel(c));
        }

        /* public async Task<ActionResult<ComponentAggregate>> Get(Guid id, DateTime? timestamp) // TODO Use `ZonedDateTime` here. Problem: Its (de)serialization is rather complex. */
        /* { */
        /*     var component = await _queryBus.Send<Component.Get.Query, ComponentView>( */
        /*         new Component.Get.Query() */
        /*         { */
        /*             ComponentId = id, */
        /*             Timestamp = timestamp, */
        /*         } */
        /*         ); */
        /*     if (component == null) */
        /*     { */
        /*         return NotFound(); */
        /*     } */
        /*     return component; */
        /* } */
    }
}