using CSharpFunctionalExtensions;
using Guid = System.Guid;
using Uri = System.Uri;

namespace Icon.Events
{
    public sealed class DatabaseCreated
      : CreatedEvent
    {
        public static DatabaseCreated From(
            Guid databaseId,
            Commands.Create<ValueObjects.CreateDatabaseInput> command
            )
        {
            return new DatabaseCreated(
                databaseId: databaseId,
                name: command.Input.Name,
                description: command.Input.Description,
                locator: command.Input.Locator,
                creatorId: command.CreatorId
                );
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Locator { get; set; }

#nullable disable
        public DatabaseCreated() { }
#nullable enable

        public DatabaseCreated(
            Guid databaseId,
            string name,
            string description,
            Uri locator,
            Guid creatorId
            )
          : base(
              aggregateId: databaseId,
              creatorId: creatorId
              )
        {
            Name = name;
            Description = description;
            Locator = locator;
            EnsureValid();
        }

        public override Result<bool, Errors> Validate()
        {
            return
              Result.Combine(
                  base.Validate(),
                  ValidateNonNull(Name, nameof(Name)),
                  ValidateNonNull(Description, nameof(Description)),
                  ValidateNonNull(Locator, nameof(Locator))
                  );
        }
    }
}