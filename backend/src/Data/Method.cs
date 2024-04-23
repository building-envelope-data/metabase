using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NpgsqlTypes;

namespace Metabase.Data;

public sealed class Method
    : Entity
{
    [Required] [MinLength(1)] public string Name { get; private set; }

    [Required] [MinLength(1)] public string Description { get; private set; }

    // Standard, being an owned type, is included by default as told on https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#querying-owned-types
    public Standard? Standard { get; set; }

    // Publication, being an owned type, is included by default as told on https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#querying-owned-types
    public Publication? Publication { get; set; }

    // TODO Make sure that either `Standard` or `Publication` is set but never both!
    [NotMapped] public IReference? Reference => Standard is not null ? Standard : Publication;

    // TODO additionalReferences (that is, standards or publications)
    // TODO service
    // TODO Description of named parameters and sources?

    public NpgsqlRange<DateTime>?
        Validity
    {
        get;
        private set;
    } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

    public NpgsqlRange<DateTime>?
        Availability
    {
        get;
        private set;
    } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

    [Url] public Uri? CalculationLocator { get; private set; }

    [Required] public Enumerations.MethodCategory[] Categories { get; private set; }

    public ICollection<InstitutionMethodDeveloper> InstitutionDeveloperEdges { get; } =
        new List<InstitutionMethodDeveloper>();

    public ICollection<Institution> InstitutionDevelopers { get; } = new List<Institution>();

    public ICollection<UserMethodDeveloper> UserDeveloperEdges { get; } = new List<UserMethodDeveloper>();
    public ICollection<User> UserDevelopers { get; } = new List<User>();

    [NotMapped]
    public IEnumerable<IStakeholder> Developers =>
        InstitutionDevelopers.Cast<IStakeholder>().Concat(
            UserDevelopers.Cast<IStakeholder>()
        );

    public Guid ManagerId { get; set; }

    [InverseProperty(nameof(Institution.ManagedMethods))]
    public Institution? Manager { get; set; }

#nullable disable
    public Method()
    {
        // Parameterless constructor is needed by HotChocolate's `UseProjection`
    }
#nullable enable

    public Method(
        string name,
        string description,
        NpgsqlRange<DateTime>? validity,
        NpgsqlRange<DateTime>? availability,
        Uri? calculationLocator,
        Enumerations.MethodCategory[] categories
    )
    {
        Name = name;
        Description = description;
        Validity = validity;
        Availability = availability;
        CalculationLocator = calculationLocator;
        Categories = categories;
    }

    public void Update(
        string name,
        string description,
        NpgsqlRange<DateTime>? validity,
        NpgsqlRange<DateTime>? availability,
        Uri? calculationLocator,
        Enumerations.MethodCategory[] categories
    )
    {
        Name = name;
        Description = description;
        Validity = validity;
        Availability = availability;
        CalculationLocator = calculationLocator;
        Categories = categories;
    }
}