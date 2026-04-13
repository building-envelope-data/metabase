using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Metabase.Enumerations;
using NodaTime;
using NpgsqlTypes;

namespace Metabase.Data;

public sealed class Method
    : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Method()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Parameterless constructor is needed by HotChocolate's `UseProjection`
    }

    public Method(
        string name,
        string description,
        NpgsqlRange<OffsetDateTime>? validity,
        NpgsqlRange<OffsetDateTime>? availability,
        Uri? calculationLocator,
        ICollection<MethodParameter> parameters,
        ICollection<MethodSource> sources,
        MethodCategory[] categories
    )
    {
        Name = name;
        Description = description;
        Validity = validity;
        Availability = availability;
        CalculationLocator = calculationLocator;
        Parameters = parameters;
        Sources = sources;
        Categories = categories;
    }

    [Required][MinLength(1)] public string Name { get; private set; }

    [Required][MinLength(1)] public string Description { get; private set; }

    // Reference, being an owned type, is included by default as told on https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#querying-owned-types
    public Reference? Reference { get; set; }

    // TODO additionalReferences (that is, standards or publications)
    // TODO service

    public NpgsqlRange<OffsetDateTime>?
        Validity
    {
        get;
        private set;
    } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

    public NpgsqlRange<OffsetDateTime>?
        Availability
    {
        get;
        private set;
    } // Inifinite bounds: https://github.com/npgsql/efcore.pg/issues/570#issuecomment-437119937 and https://www.npgsql.org/doc/api/NpgsqlTypes.NpgsqlRange-1.html#NpgsqlTypes_NpgsqlRange_1__ctor__0_System_Boolean_System_Boolean__0_System_Boolean_System_Boolean_

    [Url] public Uri? CalculationLocator { get; private set; }

    public ICollection<MethodParameter> Parameters { get; private set; } = [];
    public ICollection<MethodSource> Sources { get; private set; } = [];

    [Required] public MethodCategory[] Categories { get; private set; }

    public ICollection<InstitutionMethodDeveloper> InstitutionDeveloperEdges { get; } = [];

    public ICollection<Institution> InstitutionDevelopers { get; } = [];

    public ICollection<UserMethodDeveloper> UserDeveloperEdges { get; } = [];
    public ICollection<User> UserDevelopers { get; } = [];

    [NotMapped]
    public IEnumerable<IStakeholder> Developers =>
        InstitutionDevelopers.Concat(
            UserDevelopers.Cast<IStakeholder>()
        );

    public Guid ManagerId { get; set; }

    [InverseProperty(nameof(Institution.ManagedMethods))]
    public Institution? Manager { get; set; }

    public void Update(
        string name,
        string description,
        NpgsqlRange<OffsetDateTime>? validity,
        NpgsqlRange<OffsetDateTime>? availability,
        Uri? calculationLocator,
        ICollection<MethodParameter> parameters,
        ICollection<MethodSource> sources,
        MethodCategory[] categories
    )
    {
        Name = name;
        Description = description;
        Validity = validity;
        Availability = availability;
        CalculationLocator = calculationLocator;
        Parameters = parameters;
        Sources = sources;
        Categories = categories;
    }
}