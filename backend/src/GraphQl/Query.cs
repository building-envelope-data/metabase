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

        // TODO Use `EnableRelaySupport` in `Icon.Configuration.GraphQl` instead
        public Task<Node> GetNode(
            Guid id,
            DateTime? timestamp,
            [DataLoader] NodeForTimestampedIdDataLoader nodeLoader,
            IResolverContext resolverContext
            )
        {
            return nodeLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        public Task<IReadOnlyList<Component>> GetComponents(
            DateTime? timestamp,
            [DataLoader] ComponentsAtTimestampDataLoader componentsLoader,
            IResolverContext resolverContext
            )
        {
            return componentsLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Component> GetComponent(
            Guid id,
            DateTime? timestamp,
            [DataLoader] ComponentForTimestampedIdDataLoader componentLoader,
            IResolverContext resolverContext
            )
        {
            return componentLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        public Task<IReadOnlyList<Database>> GetDatabases(
            DateTime? timestamp,
            [DataLoader] DatabasesAtTimestampDataLoader databasesLoader,
            IResolverContext resolverContext
            )
        {
            return databasesLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Database> GetDatabase(
            Guid id,
            DateTime? timestamp,
            [DataLoader] DatabaseForTimestampedIdDataLoader databaseLoader,
            IResolverContext resolverContext
            )
        {
            return databaseLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        public Task<IReadOnlyList<Institution>> GetInstitutions(
            DateTime? timestamp,
            [DataLoader] InstitutionsAtTimestampDataLoader institutionsLoader,
            IResolverContext resolverContext
            )
        {
            return institutionsLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Institution> GetInstitution(
            Guid id,
            DateTime? timestamp,
            [DataLoader] InstitutionForTimestampedIdDataLoader institutionLoader,
            IResolverContext resolverContext
            )
        {
            return institutionLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        public Task<IReadOnlyList<Method>> GetMethods(
            DateTime? timestamp,
            [DataLoader] MethodsAtTimestampDataLoader methodsLoader,
            IResolverContext resolverContext
            )
        {
            return methodsLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Method> GetMethod(
            Guid id,
            DateTime? timestamp,
            [DataLoader] MethodForTimestampedIdDataLoader methodLoader,
            IResolverContext resolverContext
            )
        {
            return methodLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        public Task<IReadOnlyList<Person>> GetPersons(
            DateTime? timestamp,
            [DataLoader] PersonsAtTimestampDataLoader personsLoader,
            IResolverContext resolverContext
            )
        {
            return personsLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Person> GetPerson(
            Guid id,
            DateTime? timestamp,
            [DataLoader] PersonForTimestampedIdDataLoader personLoader,
            IResolverContext resolverContext
            )
        {
            return personLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }

        /* public Task<IReadOnlyList<StakeholderBase>> GetStakeholders( */
        /*     DateTime? timestamp, */
        /*     IResolverContext resolverContext */
        /*     ) */
        /* { */
        /*   return null!; */
        /* } */

        public Task<IReadOnlyList<Standard>> GetStandards(
            DateTime? timestamp,
            [DataLoader] StandardsAtTimestampDataLoader standardsLoader,
            IResolverContext resolverContext
            )
        {
            return standardsLoader.LoadAsync(
                HandleTimestamp(timestamp, resolverContext)
                );
        }

        public Task<Standard> GetStandard(
            Guid id,
            DateTime? timestamp,
            [DataLoader] StandardForTimestampedIdDataLoader standardLoader,
            IResolverContext resolverContext
            )
        {
            return standardLoader.LoadAsync(
                TimestampId(id, timestamp, resolverContext)
                );
        }
    }
}