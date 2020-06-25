namespace Icon.GraphQl
{
    public sealed class InstitutionOperatedDatabase
      : NodeBase
    {
        public static InstitutionOperatedDatabase FromModel(
            Models.InstitutionOperatedDatabase model,
            ValueObjects.Timestamp requestTimestamp
            )
        {
            return new InstitutionOperatedDatabase(
                id: model.Id,
                institutionId: model.InstitutionId,
                databaseId: model.DatabaseId,
                timestamp: model.Timestamp,
                requestTimestamp: requestTimestamp
                );
        }

        public ValueObjects.Id InstitutionId { get; }
        public ValueObjects.Id DatabaseId { get; }

        public InstitutionOperatedDatabase(
            ValueObjects.Id id,
            ValueObjects.Id institutionId,
            ValueObjects.Id databaseId,
            ValueObjects.Timestamp timestamp,
            ValueObjects.Timestamp requestTimestamp
            )
          : base(
              id: id,
              timestamp: timestamp,
              requestTimestamp: requestTimestamp
              )
        {
            InstitutionId = institutionId;
            DatabaseId = databaseId;
        }
    }
}