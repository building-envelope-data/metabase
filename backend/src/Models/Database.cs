using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
namespace Icon.Models
{
    public class Database
      : Model
    {
        public string Name { get; }
        public string Description { get; }
        public Uri Locator { get; }
        public Guid InstitutionId { get; }

        public Database(
            Guid id,
            string name,
            string description,
            Uri locator,
            Guid institutionId,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Description = description;
            Locator = locator;
            InstitutionId = institutionId;
        }
    }
}