using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using DateTime = System.DateTime;

#nullable enable
#nullable enable
namespace Icon.Models
{
    public class MethodInformation
    {
        public string Name { get; }
        public string Description { get; }
        public Guid? StandardId { get; }
        public Uri? PublicationLocator { get; }
        public Uri? CodeLocator { get; }
        public IEnumerable<MethodCategory> Categories { get; }

        public MethodInformation(
            string name,
            string description,
            Guid? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IEnumerable<MethodCategory> categories
            )
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