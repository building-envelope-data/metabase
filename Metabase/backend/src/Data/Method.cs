using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Metabase.Data
{
    public sealed class Method
      : Infrastructure.Data.Entity
    {
        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [Required]
        [MinLength(1)]
        public string Description { get; private set; }

        public Guid? StandardId { get; set; }

        public Standard? Standard { get; set; }

        [Url]
        public Uri? PublicationLocator { get; private set; }

        [Url]
        public Uri? CodeLocator { get; private set; }

        [Required]
        public Enumerations.MethodCategory[] Categories { get; private set; }

        public ICollection<InstitutionMethodDeveloper> InstitutionDeveloperEdges { get; } = new List<InstitutionMethodDeveloper>();
        public ICollection<Institution> InstitutionDevelopers { get; } = new List<Institution>();

        public ICollection<PersonMethodDeveloper> PersonDeveloperEdges { get; } = new List<PersonMethodDeveloper>();
        public ICollection<Person> PersonDevelopers { get; } = new List<Person>();

        [NotMapped]
        public IEnumerable<Stakeholder> Developers
        {
            get => InstitutionDevelopers.Cast<Stakeholder>().Concat(
                PersonDevelopers.Cast<Stakeholder>()
                );
        }

#nullable disable
        public Method()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Method(
            string name,
            string description,
            Uri? publicationLocator,
            Uri? codeLocator,
            Enumerations.MethodCategory[] categories

            )
        {
            Name = name;
            Description = description;
            PublicationLocator = publicationLocator;
            CodeLocator = codeLocator;
            Categories = categories;

        }
    }
}