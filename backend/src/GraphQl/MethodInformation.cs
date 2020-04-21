using System.Collections.Generic;
using Uri = System.Uri;
using Guid = System.Guid;
using Models = Icon.Models;
using DateTime = System.DateTime;

namespace Icon.GraphQl
{
    public sealed class MethodInformation
    {
        public static MethodInformation FromModel(
            ValueObjects.MethodInformation model
            )
        {
            return new MethodInformation(
                name: model.Name,
                description: model.Description,
                standardId: model.StandardId?.Value,
                publicationLocator: model.PublicationLocator?.Value,
                codeLocator: model.CodeLocator?.Value,
                categories: model.Categories
                );
        }

        public string Name { get; }
        public string Description { get; }
        public Guid? StandardId { get; }
        public Uri? PublicationLocator { get; }
        public Uri? CodeLocator { get; }
        public IEnumerable<ValueObjects.MethodCategory> Categories { get; }

        public MethodInformation(
            string name,
            string description,
            Guid? standardId,
            Uri? publicationLocator,
            Uri? codeLocator,
            IEnumerable<ValueObjects.MethodCategory> categories
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