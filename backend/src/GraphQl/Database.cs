using Uri = System.Uri;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class Database
      : NodeBase
    {
        public static Database FromModel(
            Models.Database model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new Database(
                id: model.Id,
                name: model.Name,
                description: model.Description,
                locator: model.Locator,
                institutionId: model.InstitutionId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }
        public ValueObjects.Id InstitutionId { get; }

        public Database(
            ValueObjects.Id id,
            string name,
            string description,
            Uri locator,
            ValueObjects.Id institutionId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }
    }
}