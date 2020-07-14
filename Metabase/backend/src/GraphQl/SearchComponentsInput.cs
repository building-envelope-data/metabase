using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.GraphQl
{
    public sealed class SearchComponentsInput
    {
        public string? Name { get; }
        public string? Abbreviation { get; }
        public string? Description { get; }

        public SearchComponentsInput(
            string? name,
            string? abbreviation,
            string? description
            )
        {
            Name = name;
            Abbreviation = abbreviation;
            Description = description;
        }

        public static Result<ValueObjects.SearchComponentsInput, Errors> Validate(
            CreateMethodInput self,
            IReadOnlyList<object> path
            )
        {
            return ValueObjects.SearchComponentsInput.From(path);
        }
    }
}