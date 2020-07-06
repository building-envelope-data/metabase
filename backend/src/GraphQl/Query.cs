using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using Icon.Infrastructure.Queries;
using Microsoft.AspNetCore.Identity;

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

        //////////
        // Data //
        //////////

        // OpticalData //

        public Task<IReadOnlyList<OpticalData>> GetOpticalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] OpticalDataOfComponentDataLoader opticalDataLoader,
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

        /* public async Task<IReadOnlyList<object>> GetOpticalData( */
        /*     ValueObjects.Id componentId, */
        /*     ValueObjects.Timestamp? timestamp, */
        /*     [DataLoader] OpticalDataOfComponentDataLoader opticalDataLoader */
        /*     ) */
        /* { */
        /*   return (await */
        /*       opticalDataLoader.LoadAsync( */
        /*         TimestampHelpers.TimestampId( */
        /*           componentId, */
        /*           timestamp ?? TimestampHelpers.Fetch(resolverContext) */
        /*           ) */
        /*         ) */
        /*       .ConfigureAwait(false) */
        /*       ) */
        /*     .Select(opticalData => opticalData.Data) */
        /*     .ToList().AsReadOnly(); */
        /* } */

        public Task<IReadOnlyList<OpticalDataFromDatabase>> GetOpticalDataFromDatabases(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
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

        public Task<bool> HasOpticalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] HasDataForComponentDataLoader<Models.OpticalData> hasOpticalDataLoader,
            IResolverContext resolverContext
            )
        {
            return hasOpticalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasOpticalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.OpticalDataFromDatabase> whoHasOpticalDataLoader,
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

        public Task<IReadOnlyList<CalorimetricData>> GetCalorimetricData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] CalorimetricDataOfComponentDataLoader calorimetricDataLoader,
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

        public Task<IReadOnlyList<CalorimetricDataFromDatabase>> GetCalorimetricDataFromDatabases(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
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

        public Task<bool> HasCalorimetricData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] HasDataForComponentDataLoader<Models.CalorimetricData> hasCalorimetricDataLoader,
            IResolverContext resolverContext
            )
        {
            return hasCalorimetricDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasCalorimetricData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.CalorimetricDataFromDatabase> whoHasCalorimetricDataLoader,
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

        public Task<IReadOnlyList<PhotovoltaicData>> GetPhotovoltaicData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] PhotovoltaicDataOfComponentDataLoader photovoltaicDataLoader,
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

        public Task<IReadOnlyList<PhotovoltaicDataFromDatabase>> GetPhotovoltaicDataFromDatabases(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
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

        public Task<bool> HasPhotovoltaicData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] HasDataForComponentDataLoader<Models.PhotovoltaicData> hasPhotovoltaicDataLoader,
            IResolverContext resolverContext
            )
        {
            return hasPhotovoltaicDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasPhotovoltaicData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.PhotovoltaicDataFromDatabase> whoHasPhotovoltaicDataLoader,
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

        public Task<IReadOnlyList<HygrothermalData>> GetHygrothermalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] HygrothermalDataOfComponentDataLoader hygrothermalDataLoader,
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

        public Task<IReadOnlyList<HygrothermalDataFromDatabase>> GetHygrothermalDataFromDatabases(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
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

        public Task<bool> HasHygrothermalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] HasDataForComponentDataLoader<Models.HygrothermalData> hasHygrothermalDataLoader,
            IResolverContext resolverContext
            )
        {
            return hasHygrothermalDataLoader.LoadAsync(
                TimestampHelpers.TimestampId(
                  componentId,
                  timestamp ?? TimestampHelpers.Fetch(resolverContext)
                  )
                );
        }

        public Task<IReadOnlyList<Database>> GetWhoHasHygrothermalData(
            ValueObjects.Id componentId,
            ValueObjects.Timestamp? timestamp,
            [DataLoader] WhichDatabasesHaveDataForComponentDataLoader<Models.HygrothermalDataFromDatabase> whoHasHygrothermalDataLoader,
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

        public IReadOnlyList<ValueObjects.Id> GetSearchComponents(
            SearchComponentsInput input
            )
        {
            // TODO !
            return new ValueObjects.Id[] { };
        }

        ////////////
        // Models //
        ////////////

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

        public Task<IReadOnlyList<User>> GetUsers(
            ValueObjects.Timestamp? timestamp,
            [DataLoader] UsersAtTimestampDataLoader usersLoader,
            IResolverContext resolverContext
            )
        {
            return usersLoader.LoadAsync(
                timestamp ?? TimestampHelpers.Fetch(resolverContext)
                );
        }

        public Task<User> GetUser(
            ValueObjects.Id id,
            ValueObjects.Timestamp? timestamp,
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