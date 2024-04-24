using System;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using Metabase.Enumerations;
using Microsoft.EntityFrameworkCore;

namespace Metabase.Data;

[Owned]
[GraphQLDescription(
    "`ISO 52022` is an example of the abbreviation of a standardizer and the main number of the identifier.")]
public sealed class Standard
    : IReference
{
    public Standard(
        string? title,
        string? @abstract,
        string? section,
        int? year,
        Standardizer[] standardizers,
        Uri? locator
    )
    {
        Title = title;
        Abstract = @abstract;
        Section = section;
        Year = year;
        Standardizers = standardizers;
        Locator = locator;
    }

    [Range(0, 3000)]
    [GraphQLDescription(
        "It is important to define the year in which the standard was issued because there can be relevant updates of one standard.")]
    public int? Year { get; private set; }

    // Numeration, being an owned type, is included by default as told on https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities#querying-owned-types
    [Required] public Numeration Numeration { get; set; } = default!;

    [Required] public Standardizer[] Standardizers { get; private set; }

    [Url] public Uri? Locator { get; private set; }
    [MinLength(1)] public string? Title { get; }

    [MinLength(1)] public string? Abstract { get; }

    [MinLength(1)]
    [GraphQLDescription("The section of the standard to which the reference refers to.")]
    public string? Section { get; }
}