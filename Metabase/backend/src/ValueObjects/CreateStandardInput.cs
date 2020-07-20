using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Errors = Infrastructure.Errors;

namespace Metabase.ValueObjects
{
    public sealed class CreateStandardInput
      : ValueObject
    {
        public Title Title { get; }
        public Abstract Abstract { get; }
        public Section Section { get; }
        public Year Year { get; }
        public Numeration Numeration { get; }
        public IReadOnlyCollection<Standardizer> Standardizers { get; }
        public AbsoluteUri? Locator { get; }

        private CreateStandardInput(
            Title title,
            Abstract @abstract,
            Section section,
            Year year,
            Numeration numeration,
            IReadOnlyCollection<Standardizer> standardizers,
            AbsoluteUri? locator
            )
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Year = year;
            Numeration = numeration;
            Standardizers = standardizers;
            Locator = locator;
        }

        public static Result<CreateStandardInput, Errors> From(
            Title title,
            Abstract @abstract,
            Section section,
            Year year,
            Numeration numeration,
            IReadOnlyCollection<Standardizer> standardizers,
            AbsoluteUri? locator
            )
        {
            return
              Result.Success<CreateStandardInput, Errors>(
                  new CreateStandardInput(
                    title: title,
                    @abstract: @abstract,
                    section: section,
                    year: year,
                    numeration: numeration,
                    standardizers: standardizers,
                    locator: locator
                    )
                  );
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Title;
            yield return Abstract;
            yield return Section;
            yield return Year;
            yield return Numeration;
            foreach (var standardizer in Standardizers)
            {
                yield return standardizer;
            }
            yield return Locator;
        }
    }
}