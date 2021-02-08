using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public sealed class Institution
      : Stakeholder
    {
        [Required]
        [MinLength(1)]
        public string? Name { get; set; }

        [MinLength(1)]
        public string? Abbreviation { get; set; }

        [Required]
        [MinLength(1)]
        public string? Description { get; set; }

        [Url]
        public string? WebsiteLocator { get; set; }

        [MinLength(1)]
        public string? PublicKey { get; set; }

        [Required]
        public Enumerations.InstitutionState? State { get; set; }

        public ICollection<PersonAffiliation> AffiliatedPersonEdges { get; } = new List<PersonAffiliation>();
        public ICollection<Person> AffiliatedPersons { get; } = new List<Person>();

        public ICollection<InstitutionMethodDeveloper> DevelopedMethodEdges { get; } = new List<InstitutionMethodDeveloper>();
        public ICollection<Method> DevelopedMethods { get; } = new List<Method>();

        public ICollection<ComponentManufacturer> ManufacturedComponentEdges { get; } = new List<ComponentManufacturer>();
        public ICollection<Component> ManufacturedComponents { get; } = new List<Component>();

        [InverseProperty(nameof(Database.Operator))]
        public ICollection<Database> OperatedDatabases { get; } = new List<Database>();

        public ICollection<InstitutionRepresentative> RepresentativeEdges { get; } = new List<InstitutionRepresentative>();
        public ICollection<User> Representatives { get; } = new List<User>();

        public Institution()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }

        public Institution(
            string name,
            string? abbreviation,
            string description,
            string? websiteLocator,
            string? publicKey,
            Enumerations.InstitutionState state
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
            State = state;
        }

        public void Update(
            string name,
            string? abbreviation,
            string description,
            string? websiteLocator,
            string? publicKey,
            Enumerations.InstitutionState state
        )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
            State = state;
        }
    }
}