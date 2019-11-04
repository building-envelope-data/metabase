using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;

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

        public async Task<IEnumerable<Component>> GetComponents()
        {
            return
              (await _queryBus
               .Send<
                  Queries.ListComponents,
                  IEnumerable<Models.Component>
                  >(new Queries.ListComponents()))
              .Select(c => Component.FromModel(c));
        }

        public async Task<Component> GetComponent(Guid id, DateTime? timestamp)
        {
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