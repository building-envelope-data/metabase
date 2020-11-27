using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Infrastructure.GraphQl
{
    public abstract class SearchComponentResult<TOpticalData, TCalorimetricData, TPhotovoltaicData, THygrothermalData>
    {
        public Id Id { get; }
        public IReadOnlyList<TOpticalData> OpticalData { get; }
        public IReadOnlyList<TCalorimetricData> CalorimetricData { get; }
        public IReadOnlyList<TPhotovoltaicData> PhotovoltaicData { get; }
        public IReadOnlyList<THygrothermalData> HygrothermalData { get; }
        public Timestamp RequestTimestamp { get; }

        protected SearchComponentResult(
            Id id,
            IReadOnlyList<TOpticalData> opticalData,
            IReadOnlyList<TCalorimetricData> calorimetricData,
            IReadOnlyList<TPhotovoltaicData> photovoltaicData,
            IReadOnlyList<THygrothermalData> hygrothermalData,
            Timestamp requestTimestamp
            )
        {
            Id = id;
            OpticalData = opticalData;
            CalorimetricData = calorimetricData;
            PhotovoltaicData = photovoltaicData;
            HygrothermalData = hygrothermalData;
            RequestTimestamp = requestTimestamp;
        }
    }
}
