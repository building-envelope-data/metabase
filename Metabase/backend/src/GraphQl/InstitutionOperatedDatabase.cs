using Infrastructure.ValueObjects;
namespace Metabase.GraphQl
{
    public sealed class InstitutionOperatedDatabase
      : NodeBase
    {
        public static InstitutionOperatedDatabase FromModel(
            Models.InstitutionOperatedDatabase model,
            Timestamp requestTimestamp
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

        public Id InstitutionId { get; }
        public Id DatabaseId { get; }

        public InstitutionOperatedDatabase(
            Id id,
            Id institutionId,
            Id databaseId,
            Timestamp timestamp,
            Timestamp requestTimestamp
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