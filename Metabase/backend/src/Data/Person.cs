using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Metabase.Data
{
    public sealed class Person
      : Stakeholder
    {
        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [Required]
        public ContactInformation ContactInformation { get; set; } = default!;

        public ICollection<PersonAffiliation> AffiliatedInstitutionEdges { get; } = new List<PersonAffiliation>();
        public ICollection<Institution> AffiliatedInstitutions { get; } = new List<Institution>();

        public ICollection<PersonMethodDeveloper> DevelopedMethodEdges { get; } = new List<PersonMethodDeveloper>();
        public ICollection<Method> DevelopedMethods { get; } = new List<Method>();

#nullable disable
        public Person()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Person(
            string name
            )
        {
            Name = name;
        }
    }
}