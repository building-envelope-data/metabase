using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;

namespace Icon.GraphQl
{
    public sealed class Query
    {
        private readonly IQueryBus _queryBus;
        private readonly UserManager<Models.User> _userManager;

        public Query(IQueryBus queryBus, UserManager<Models.User> userManager)
        {
            _queryBus = queryBus;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Component>> GetComponents(DateTime? timestamp, IResolverContext context)
        {
            // TODO Is there a better way to pass data down the tree to resolvers?
            context.ScopedContextData = context.ScopedContextData.SetItem("timestamp", timestamp);
            return
              (await _queryBus
               .Send<
                  Queries.ListComponents,
                  IEnumerable<Models.Component>
                  >(new Queries.ListComponents(timestamp)))
              .Select(c => Component.FromModel(c));
        }

        public async Task<Component> GetComponent(Guid id, DateTime? timestamp, IResolverContext context)
        {
            // TODO Is there a better way to pass data down the tree to resolvers?
            context.ScopedContextData = context.ScopedContextData.SetItem("timestamp", timestamp);
            return
              Component.FromModel(
                  (await _queryBus
                    .Send<
                       Queries.GetComponent,
                       Models.Component
                       >(new Queries.GetComponent(id, timestamp))
                  ));
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