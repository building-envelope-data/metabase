using System.Collections.Generic;
using System.Collections.ObjectModel; // ReadOnlyDictionary
using CSharpFunctionalExtensions;
using DateTime = System.DateTime;
using Errors = Icon.Errors;
using ValueObjects = Icon.ValueObjects;

namespace Icon.Models
{
    public abstract class DataX<TDataJson>
      : Model
    {
        public ValueObjects.Id ComponentId { get; }
        public TDataJson Data { get; }

        protected DataX(
            ValueObjects.Id id,
            ValueObjects.Id componentId,
            TDataJson data,
            ValueObjects.Timestamp timestamp
            )
          : base(id, timestamp)
        {
            ComponentId = componentId;
            Data = data;
        }
    }
}