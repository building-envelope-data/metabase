using System.Collections.Generic;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.Resolvers;
using Infrastructure.GraphQl;
using Infrastructure.Queries;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Database.GraphQl
{
    public sealed class Query
      : Infrastructure.GraphQl.Query
    {
        public Query(IQueryBus queryBus)
          : base(queryBus)
        {
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

        public Task<IReadOnlyList<OpticalData>> GetOpticalData(
            Id componentId,
            Timestamp? timestamp,
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
        /*     Id componentId, */
        /*     Timestamp? timestamp, */
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

        public Task<bool> HasOpticalData(
            Id componentId,
            Timestamp? timestamp,
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

        // Calorimetric //

        public Task<IReadOnlyList<CalorimetricData>> GetCalorimetricData(
            Id componentId,
            Timestamp? timestamp,
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

        public Task<bool> HasCalorimetricData(
            Id componentId,
            Timestamp? timestamp,
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

        // Photovoltaic //

        public Task<IReadOnlyList<PhotovoltaicData>> GetPhotovoltaicData(
            Id componentId,
            Timestamp? timestamp,
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

        public Task<bool> HasPhotovoltaicData(
            Id componentId,
            Timestamp? timestamp,
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

        // Hygrothermal //

        public Task<IReadOnlyList<HygrothermalData>> GetHygrothermalData(
            Id componentId,
            Timestamp? timestamp,
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

        public Task<bool> HasHygrothermalData(
            Id componentId,
            Timestamp? timestamp,
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

        public IReadOnlyList<TimestampedId> SearchComponents(
            SearchComponentsPropositionInput where,
            Timestamp? timestamp
            )
        {
            return new TimestampedId[] { };
        }
    }
}