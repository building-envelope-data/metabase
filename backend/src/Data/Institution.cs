using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Metabase.Data
{
    public sealed class Institution
      : Data.Entity,
        IStakeholder
    {
        [Required]
        [MinLength(1)]
        public string Name { get; private set; }

        [MinLength(1)]
        public string? Abbreviation { get; private set; }

        [Required]
        [MinLength(1)]
        public string Description { get; private set; }

        [Url]
        public Uri? WebsiteLocator { get; private set; }

        [MinLength(1)]
        public string? PublicKey { get; private set; }

        [Required]
        public Enumerations.InstitutionState State { get; private set; }

        public Enumerations.InstitutionOperatingState OperatingState { get; set; }

        public ICollection<InstitutionMethodDeveloper> DevelopedMethodEdges { get; } = new List<InstitutionMethodDeveloper>();
        public ICollection<Method> DevelopedMethods { get; } = new List<Method>();

        [InverseProperty(nameof(Method.Manager))]
        public ICollection<Method> ManagedMethods { get; } = new List<Method>();

        [InverseProperty(nameof(DataFormat.Manager))]
        public ICollection<DataFormat> ManagedDataFormats { get; } = new List<DataFormat>();

        public ICollection<ComponentManufacturer> ManufacturedComponentEdges { get; } = new List<ComponentManufacturer>();
        public ICollection<Component> ManufacturedComponents { get; } = new List<Component>();

        [InverseProperty(nameof(Database.Operator))]
        public ICollection<Database> OperatedDatabases { get; } = new List<Database>();

        public Guid? ManagerId { get; set; }

        [InverseProperty(nameof(ManagedInstitutions))]
        public Institution? Manager { get; set; }

        [InverseProperty(nameof(Manager))]
        public ICollection<Institution> ManagedInstitutions { get; } = new List<Institution>();

        public ICollection<InstitutionRepresentative> RepresentativeEdges { get; } = new List<InstitutionRepresentative>();
        public ICollection<User> Representatives { get; } = new List<User>();

#nullable disable
        public Institution()
        {
            // Parameterless constructor is needed by HotChocolate's `UseProjection`
        }
#nullable enable

        public Institution(
            string name,
            string? abbreviation,
            string description,
            Uri? websiteLocator,
            string? publicKey,
            Enumerations.InstitutionOperatingState operatingState,
            Enumerations.InstitutionState state
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
            State = state;
            OperatingState = operatingState;
        }

        public void Update(
            string name,
            string? abbreviation,
            string description,
            Uri? websiteLocator,
            string? publicKey
        )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
            WebsiteLocator = websiteLocator;
            PublicKey = publicKey;
        }

        public void Verify()
        {
            State = Enumerations.InstitutionState.VERIFIED;
        }
    }
}