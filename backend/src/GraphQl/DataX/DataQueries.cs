using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Guid = System.Guid;

namespace Metabase.GraphQl.DataX
{
    [ExtendObjectType(nameof(Query))]
    public sealed class DataQueries
    {
        [UseDbContext(typeof(Data.ApplicationDbContext))]
        [UsePaging]
        // [UseProjection] // We disabled projections because when requesting `id` all results had the same `id` and when also requesting `uuid`, the latter was always the empty UUID `000...`.
        [UseFiltering]
        [UseSorting]
        public IQueryable<DataX> GetAllData(
            [ScopedService] Data.ApplicationDbContext context
            )
        {
            return null!;
        }

        // public Task<Data.Data?> GetDataAsync(
        //     Guid uuid,
        //     CancellationToken cancellationToken
        //     )
        // {
        // }

      public DataX Data { get; set; }
      public CalorimetricData CalorimetricData { get; set; }
      public HygrothermalData HygrothermalData { get; set; }
      public OpticalData OpticalData { get; set; }
      public PhotovoltaicData PhotovoltaicData { get; set; }
      public DataConnection AllData { get; set; }
      public CalorimetricDataConnection AllCalorimetricData { get; set; }
      public HygrothermalDataConnection AllHygrothermalData { get; set; }
      public OpticalDataConnection AllOpticalData { get; set; }
      public PhotovoltaicDataConnection AllPhotovoltaicData { get; set; }
      public bool HasData { get; set; }
      public bool HasCalorimetricData { get; set; }
      public bool HasHygrothermalData { get; set; }
      public bool HasOpticalData { get; set; }
      public bool HasPhotovoltaicData { get; set; }
    }
}