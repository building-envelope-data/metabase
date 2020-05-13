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
    {
        private readonly IQueryBus _queryBus;
        private readonly UserManager<Models.UserX> _userManager;

        public Query(IQueryBus queryBus, UserManager<Models.UserX> userManager)
        {
            _queryBus = queryBus;
            _userManager = userManager;
        }

        // TODO Use `EnableRelaySupport` in `Icon.Configuration.GraphQl` instead
        public Task<Node> GetNode(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] NodeDataLoader nodeLoader,
            IResolverContext resolverContext
            )
        {
            return nodeLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Component>> GetComponents(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] ComponentsAtTimestampDataLoader componentsLoader,
            IResolverContext resolverContext
            )
        {
            return componentsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Component> GetComponent(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] ComponentDataLoader componentLoader,
            IResolverContext resolverContext
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Database>> GetDatabases(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] DatabasesAtTimestampDataLoader databasesLoader,
            IResolverContext resolverContext
            )
        {
            return databasesLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Database> GetDatabase(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] DatabaseDataLoader databaseLoader,
            IResolverContext resolverContext
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Institution>> GetInstitutions(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] InstitutionsAtTimestampDataLoader institutionsLoader,
            IResolverContext resolverContext
            )
        {
            return institutionsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Institution> GetInstitution(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] InstitutionDataLoader institutionLoader,
            IResolverContext resolverContext
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Method>> GetMethods(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] MethodsAtTimestampDataLoader methodsLoader,
            IResolverContext resolverContext
            )
        {
            return methodsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Method> GetMethod(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] MethodDataLoader methodLoader,
            IResolverContext resolverContext
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Person>> GetPersons(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] PersonsAtTimestampDataLoader personsLoader,
            IResolverContext resolverContext
            )
        {
            return personsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Person> GetPerson(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] PersonDataLoader personLoader,
            IResolverContext resolverContext
            )
        {
            return personLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        // TODO GetStakeholder and GetStakeholders

        public Task<IReadOnlyList<Standard>> GetStandards(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] StandardsAtTimestampDataLoader standardsLoader,
            IResolverContext resolverContext
            )
        {
            return standardsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Standard> GetStandard(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] StandardDataLoader standardLoader,
            IResolverContext resolverContext
            )
        {
            return standardLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }
    }
}