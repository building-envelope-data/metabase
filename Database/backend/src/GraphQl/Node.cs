using Infrastructure.GraphQl;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Exception = System.Exception;

namespace Database.GraphQl
{
    public abstract class Node
      : Infrastructure.GraphQl.Node
    {
        public static INode FromModel(
            IModel model,
            Timestamp requestTimestamp
            )
        {
            return model switch
            {
                Models.CalorimetricData calorimetricData =>
                  CalorimetricData.FromModel(calorimetricData, requestTimestamp),
                Models.HygrothermalData hygrothermalData =>
                  HygrothermalData.FromModel(hygrothermalData, requestTimestamp),
                Models.OpticalData opticalData =>
                  OpticalData.FromModel(opticalData, requestTimestamp),
                Models.PhotovoltaicData photovoltaicData =>
                  PhotovoltaicData.FromModel(photovoltaicData, requestTimestamp),
                _ =>
                  throw new Exception($"The model {model} fell through")
            };
        }

        protected Node(
            Id id,
            Timestamp timestamp,
            Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
        }
    }
}