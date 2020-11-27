using Infrastructure.Events;
using Newtonsoft.Json;
using Guid = System.Guid;

namespace Metabase.Events
{
    public sealed class InstitutionOperatedDatabaseAdded
      : AssociationAddedEvent
    {
        public static InstitutionOperatedDatabaseAdded From(
            Guid institutionOperatedDatabaseId,
            Guid databaseId,
            Infrastructure.Commands.CreateCommand<ValueObjects.CreateDatabaseInput> command
            )
        {
            return new InstitutionOperatedDatabaseAdded(
                institutionOperatedDatabaseId: institutionOperatedDatabaseId,
                institutionId: command.Input.InstitutionId,
                databaseId: databaseId,
                creatorId: command.CreatorId
                );
        }

        public static InstitutionOperatedDatabaseAdded From(
            Guid institutionOperatedDatabaseId,
            Infrastructure.Commands.AddAssociationCommand<ValueObjects.AddInstitutionOperatedDatabaseInput> command
            )
        {
            return new InstitutionOperatedDatabaseAdded(
                institutionOperatedDatabaseId: institutionOperatedDatabaseId,
                institutionId: command.Input.InstitutionId,
                databaseId: command.Input.DatabaseId,
                creatorId: command.CreatorId
                );
        }

        [JsonIgnore]
        public Guid InstitutionId { get => ParentId; }

        [JsonIgnore]
        public Guid DatabaseId { get => AssociateId; }

#nullable disable
        public InstitutionOperatedDatabaseAdded() { }
#nullable enable

        public InstitutionOperatedDatabaseAdded(
            Guid institutionOperatedDatabaseId,
            Guid institutionId,
            Guid databaseId,
            Guid creatorId
            )
          : base(
              aggregateId: institutionOperatedDatabaseId,
              parentId: institutionId,
              associateId: databaseId,
              creatorId: creatorId
              )
        {
            EnsureValid();
        }
    }
}