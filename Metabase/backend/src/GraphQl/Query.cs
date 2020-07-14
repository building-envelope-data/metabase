using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Metabase.GraphQl
{
    public sealed class Query
      : Infrastructure.GraphQl.Query
    {
        private readonly UserManager<Models.UserX> _userManager;

        public Query(IQueryBus queryBus, UserManager<Models.UserX> userManager)
          : base(queryBus)
        {
            _userManager = userManager;
        }

        // TODO Use `EnableRelaySupport` in `Metabase.Configuration.GraphQl` instead
        public Task<Node> GetNode(
            Id id,
            Timestamp? timestamp,
            [DataLoader] NodeDataLoader nodeLoader,
            IResolverContext resolverContext
            )
        {
            return nodeLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        //////////
        // Data //
        //////////

        // OpticalData //

        public Task<IReadOnlyList<OpticalData>> GetOpticalDataFromDatabases(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] OpticalDataOfComponentFromDatabasesDataLoader opticalDataLoader,
            IResolverContext resolverContext
            )
        {
            return opticalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasOpticalData(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.OpticalData> whoHasOpticalDataLoader,
            IResolverContext resolverContext
            )
        {
            return whoHasOpticalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        // Calorimetric //

        public Task<IReadOnlyList<CalorimetricData>> GetCalorimetricDataFromDatabases(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] CalorimetricDataOfComponentFromDatabasesDataLoader calorimetricDataLoader,
            IResolverContext resolverContext
            )
        {
            return calorimetricDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasCalorimetricData(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.CalorimetricData> whoHasCalorimetricDataLoader,
            IResolverContext resolverContext
            )
        {
            return whoHasCalorimetricDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        // Photovoltaic //

        public Task<IReadOnlyList<PhotovoltaicData>> GetPhotovoltaicDataFromDatabases(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] PhotovoltaicDataOfComponentFromDatabasesDataLoader photovoltaicDataLoader,
            IResolverContext resolverContext
            )
        {
            return photovoltaicDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasPhotovoltaicData(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.PhotovoltaicData> whoHasPhotovoltaicDataLoader,
            IResolverContext resolverContext
            )
        {
            return whoHasPhotovoltaicDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        // Hygrothermal //

        public Task<IReadOnlyList<HygrothermalData>> GetHygrothermalDataFromDatabases(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] HygrothermalDataOfComponentFromDatabasesDataLoader hygrothermalDataLoader,
            IResolverContext resolverContext
            )
        {
            return hygrothermalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasHygrothermalData(
            Id componentId,
            Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.HygrothermalData> whoHasHygrothermalDataLoader,
            IResolverContext resolverContext
            )
        {
            return whoHasHygrothermalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public IReadOnlyList<Id> GetSearchComponents(
            SearchComponentsInput input
            )
        {
            // TODO !
            return new Id[] { };
        }

        ////////////
        // Models //
        ////////////

        public Task<IReadOnlyList<Component>> GetComponents(
            Timestamp? timestamp,
            [DataLoader] ComponentsAtTimestampDataLoader componentsLoader,
            IResolverContext resolverContext
            )
        {
            return componentsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Component> GetComponent(
            Id id,
            Timestamp? timestamp,
            [DataLoader] ComponentDataLoader componentLoader,
            IResolverContext resolverContext
            )
        {
            return componentLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Database>> GetDatabases(
            Timestamp? timestamp,
            [DataLoader] DatabasesAtTimestampDataLoader databasesLoader,
            IResolverContext resolverContext
            )
        {
            return databasesLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Database> GetDatabase(
            Id id,
            Timestamp? timestamp,
            [DataLoader] DatabaseDataLoader databaseLoader,
            IResolverContext resolverContext
            )
        {
            return databaseLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Institution>> GetInstitutions(
            Timestamp? timestamp,
            [DataLoader] InstitutionsAtTimestampDataLoader institutionsLoader,
            IResolverContext resolverContext
            )
        {
            return institutionsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Institution> GetInstitution(
            Id id,
            Timestamp? timestamp,
            [DataLoader] InstitutionDataLoader institutionLoader,
            IResolverContext resolverContext
            )
        {
            return institutionLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Method>> GetMethods(
            Timestamp? timestamp,
            [DataLoader] MethodsAtTimestampDataLoader methodsLoader,
            IResolverContext resolverContext
            )
        {
            return methodsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Method> GetMethod(
            Id id,
            Timestamp? timestamp,
            [DataLoader] MethodDataLoader methodLoader,
            IResolverContext resolverContext
            )
        {
            return methodLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<Person>> GetPersons(
            Timestamp? timestamp,
            [DataLoader] PersonsAtTimestampDataLoader personsLoader,
            IResolverContext resolverContext
            )
        {
            return personsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Person> GetPerson(
            Id id,
            Timestamp? timestamp,
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
            Timestamp? timestamp,
            [DataLoader] StandardsAtTimestampDataLoader standardsLoader,
            IResolverContext resolverContext
            )
        {
            return standardsLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<Standard> GetStandard(
            Id id,
            Timestamp? timestamp,
            [DataLoader] StandardDataLoader standardLoader,
            IResolverContext resolverContext
            )
        {
            return standardLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }

        public Task<IReadOnlyList<User>> GetUsers(
            Timestamp? timestamp,
            [DataLoader] UsersAtTimestampDataLoader usersLoader,
            IResolverContext resolverContext
            )
        {
            return usersLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<User> GetUser(
            Id id,
            Timestamp? timestamp,
            [DataLoader] UserDataLoader userLoader,
            IResolverContext resolverContext
            )
        {
            return userLoader.LoadAsync(
                TimestampHelpers.TimestampId(id, timestamp ?? TimestampHelpers.Fetch(resolverContext))
                );
        }
    }
}