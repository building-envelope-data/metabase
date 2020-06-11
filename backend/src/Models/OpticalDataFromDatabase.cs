using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public sealed class OpticalDataFromDatabase
      : Model
    {
        public ValueObjects.Id DatabaseId { get; }
        public ValueObjects.Id ComponentId { get; }
        public ValueObjects.OpticalDataJson Data { get; }

        private OpticalDataFromDatabase(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.OpticalDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            DatabaseId = databaseId;
            ComponentId = componentId;
            Data = data;
        }

        public static Result<OpticalDataFromDatabase, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Id databaseId,
            ValueObjects.Id componentId,
            ValueObjects.OpticalDataJson data,
            ValueObjects.Timestamp timestamp
            )
        {
            return
              Result.Ok<OpticalDataFromDatabase, Errors>(
                  new OpticalDataFromDatabase(
                    id: id,
                    databaseId: databaseId,
                    componentId: componentId,
                    data: data,
                    timestamp: timestamp
                    )
                  );
        }
    }
}