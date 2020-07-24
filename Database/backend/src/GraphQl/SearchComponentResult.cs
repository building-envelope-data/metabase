using System.Collections.Generic;
using Infrastructure.ValueObjects;

namespace Database.GraphQl
{
    public sealed class SearchComponentResult
    {
        public Id Id { get; }
        public IReadOnlyList<OpticalData> OpticalData { get; }
        public IReadOnlyList<CalorimetricData> CalorimetricData { get; }
        public IReadOnlyList<PhotovoltaicData> PhotovoltaicData { get; }
        public IReadOnlyList<HygrothermalData> HygrothermalData { get; }
        public Timestamp RequestTimestamp { get; }

        public SearchComponentResult(
            Id id,
            IReadOnlyList<OpticalData> opticalData,
            IReadOnlyList<CalorimetricData> calorimetricData,
            IReadOnlyList<PhotovoltaicData> photovoltaicData,
            IReadOnlyList<HygrothermalData> hygrothermalData,
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