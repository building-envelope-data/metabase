using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

namespace Icon.Models
{
    public class MethodInformation
      : Model
    {
        public string Name { get; }
        public string Description { get; }
        public Guid? StandardId { get; }
        public Uri? PublicationLocator { get; }
        public Uri? CodeLocator { get; }
        public IEnumerable<MethodCategory> Categories { get; }

        public MethodInformation(
            Guid id,
            string name,
            string description,
            Guid? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IEnumerable<MethodCategory> categories,
            DateTime timestamp
            )
          : base(id, timestamp)
        {
            Name = name;
            Description = description;
            StandardId = standardId;
            PublicationLocator = publicationLocator;
            CodeLocator = codeLocator;
            Categories = categories;
        }
    }
}