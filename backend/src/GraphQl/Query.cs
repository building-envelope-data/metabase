using System;
using Icon.Infrastructure.Query;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Models = Icon.Models;
using Queries = Icon.Queries;
using System.Linq;
using HotChocolate.Resolvers;
using HotChocolate;
using GreenDonut;
using QueryException = HotChocolate.Execution.QueryException;
using IError = HotChocolate.IError;
using CSharpFunctionalExtensions;

namespace Icon.GraphQl
{
    public sealed class Query
      : QueryAndMutationBase
    {
        private readonly IQueryBus _queryBus;
        private readonly UserManager<Models.User> _userManager;

        public Query(IQueryBus queryBus, UserManager<Models.User> userManager)
        {
            _queryBus = queryBus;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Component>> GetComponents(
            DateTime? timestamp,
            IResolverContext resolverContext
            )
        {
            var requestTimestamp = HandleTimestamp(timestamp, resolverContext);
            return ResultHelpers.HandleFailures<Models.Component>(
              await _queryBus
               .Send<
                  Queries.ListComponents,
                  IEnumerable<Result<Models.Component, IError>>
                  >(new Queries.ListComponents(requestTimestamp))
              )
              .Select(c => Component.FromModel(c, requestTimestamp));
        }

        public Task<Component> GetComponent(
            Guid id,
            DateTime? timestamp,
            [DataLoader] ComponentDataLoader componentLoader,
            IResolverContext resolverContext
            )
        {
            var requestTimestamp = HandleTimestamp(timestamp, resolverContext);
            return
              componentLoader.LoadAsync(
                  (id.NotEmpty(), requestTimestamp)
                  );
            /* return */
            /*   Component.FromModel( */
            /*       (await _queryBus */
            /*         .Send< */
            /*            Queries.GetComponent, */
            /*            Models.Component */
            /*            >(new Queries.GetComponent(id, nonNullTimestamp)) */
            /*       )); */
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
        /*     if (component is null) */
        /*     { */
        /*         return NotFound(); */
        /*     } */
        /*     return component; */
        /* } */
    }
}