using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Database.GraphQl
{
    public sealed class SearchComponentResult
      : Infrastructure.GraphQl.SearchComponentResult<OpticalData, CalorimetricData, PhotovoltaicData, HygrothermalData>
    {
        public SearchComponentResult(
            Id id,
            IReadOnlyList<OpticalData> opticalData,
            IReadOnlyList<CalorimetricData> calorimetricData,
            IReadOnlyList<PhotovoltaicData> photovoltaicData,
            IReadOnlyList<HygrothermalData> hygrothermalData,
            Timestamp requestTimestamp
            )
          : base(
              id,
              opticalData,
              calorimetricData,
              photovoltaicData,
              hygrothermalData,
              requestTimestamp
              )
        {
        }
    }
}