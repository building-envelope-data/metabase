using Infrastructure.GraphQl;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Database.GraphQl
{
    public abstract class NodeBase
      : Node
    {
        protected static TimestampedId TimestampId(
            Id id,
            Timestamp timestamp
            )
        {
            return ResultHelpers.HandleFailure(
                TimestampedId.From(
                  id, timestamp
                  )
                );
        }

        public static Node FromModel(
            IModel model,
            Timestamp requestTimestamp
            )
        {
            if (model is Models.CalorimetricData calorimetricData)
                return CalorimetricData.FromModel(calorimetricData, requestTimestamp);
            if (model is Models.HygrothermalData hygrothermalData)
                return HygrothermalData.FromModel(hygrothermalData, requestTimestamp);
            if (model is Models.OpticalData opticalData)
                return OpticalData.FromModel(opticalData, requestTimestamp);
            if (model is Models.PhotovoltaicData photovoltaicData)
                return PhotovoltaicData.FromModel(photovoltaicData, requestTimestamp);
            throw new Exception($"The model {model} fell through");
        }

        public Id Id { get; }
        public Timestamp Timestamp { get; }
        public Timestamp RequestTimestamp { get; }

        protected NodeBase(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
        {
            Id = id;
            Timestamp = timestamp;
            RequestTimestamp = requestTimestamp;
        }
    }
}