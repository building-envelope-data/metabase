using System;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using Metabase.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
[GraphQLDescription("`ISO 52022` is an example of the abbreviation of a standardizer and the main number of the identifier.")]
public sealed class Standard(
    string? title,
    string? @abstract,
    string? section,
    int? year,
    Standardizer[] standardizers,
    Uri? locator
    )
        : IReference
{
    [Range(0, 3000)]
    [GraphQLDescription("It is important to define the year in which the standard was issued because there can be relevant updates of one standard.")]
    public int? Year { get; private set; } = year;

    // Numeration, being an owned type, is included by default as told on https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#querying-owned-types
    [Required] public Numeration Numeration { get; set; } = default!;

    [Required] public Standardizer[] Standardizers { get; private set; } = standardizers;

    [Url] public Uri? Locator { get; private set; } = locator;
    [MinLength(1)] public string? Title { get; private set; } = title;

    [MinLength(1)] public string? Abstract { get; private set; } = @abstract;

    [MinLength(1)]
    [GraphQLDescription("The section of the standard to which the reference refers to.")]
    public string? Section { get; private set; } = section;

    // To evade the warning
    // ---
    // Entity type `Publication` is an optional dependent using table sharing and
    // containing other dependents without any required non shared property to
    // identify whether the entity exists. If all nullable properties contain a
    // null value in database then an object instance won't be created in the
    // query causing nested dependent's values to be lost. Add a required
    // property to create instances with null values for other properties or
    // mark the incoming navigation as required to always create an instance.
    // ---
    // I introduce this non-null property.
    [Required] public bool Exists { get; private set; } = true;
}