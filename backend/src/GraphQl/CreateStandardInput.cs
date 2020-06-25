using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Uri = System.Uri;

namespace Icon.GraphQl
{
    public sealed class CreateStandardInput
    {
        public string Title { get; }
        public string Abstract { get; }
        public string Section { get; }
        public int Year { get; }
        public string? Prefix { get; }
        public string MainNumber { get; }
        public string? Suffix { get; }
        public IReadOnlyCollection<ValueObjects.Standardizer> Standardizers { get; }
        public Uri? Locator { get; }

        public CreateStandardInput(
            string title,
            string @abstract,
            string section,
            int year,
            string? prefix,
            string mainNumber,
            string? suffix,
            IReadOnlyCollection<ValueObjects.Standardizer> standardizers,
            Uri? locator
            )
        {
            Title = title;
            Abstract = @abstract;
            Section = section;
            Year = year;
            Prefix = prefix;
            MainNumber = mainNumber;
            Suffix = suffix;
            Standardizers = standardizers;
            Locator = locator;
        }

        public static Result<ValueObjects.CreateStandardInput, Errors> Validate(
            CreateStandardInput self,
            IReadOnlyList<object> path
            )
        {
            var titleResult = ValueObjects.Title.From(
                self.Title,
                path.Append("title").ToList().AsReadOnly()
                );
            var abstractResult = ValueObjects.Abstract.From(
                self.Abstract,
                path.Append("abstract").ToList().AsReadOnly()
                );
            var sectionResult = ValueObjects.Section.From(
                self.Section,
                path.Append("section").ToList().AsReadOnly()
                );
            var yearResult = ValueObjects.Year.From(
                self.Year,
                path.Append("year").ToList().AsReadOnly()
                );
            var prefixResult = ValueObjects.Prefix.MaybeFrom(
                self.Prefix,
                path.Append("prefix").ToList().AsReadOnly()
                );
            var mainNumberResult = ValueObjects.MainNumber.From(
                self.MainNumber,
                path.Append("mainNumber").ToList().AsReadOnly()
                );
            var suffixResult = ValueObjects.Suffix.MaybeFrom(
                self.Suffix,
                path.Append("suffix").ToList().AsReadOnly()
                );
            var numerationResult = Errors.CombineExistent(
                prefixResult,
                mainNumberResult,
                suffixResult
                )
              .Bind(_ =>
                  ValueObjects.Numeration.From(
                    prefix: prefixResult?.Value,
                    mainNumber: mainNumberResult.Value,
                    suffix: suffixResult?.Value
                    )
                  );
            var locatorResult = ValueObjects.AbsoluteUri.MaybeFrom(
                self.Locator,
                path.Append("locator").ToList().AsReadOnly()
                );

            return
              Errors.CombineExistent(
                  titleResult,
                  abstractResult,
                  sectionResult,
                  yearResult,
                  numerationResult,
                  locatorResult
                  )
              .Bind(_ =>
                  ValueObjects.CreateStandardInput.From(
                    title: titleResult.Value,
                    @abstract: abstractResult.Value,
                    section: sectionResult.Value,
                    year: yearResult.Value,
                    numeration: numerationResult.Value,
                    standardizers: self.Standardizers,
                    locator: locatorResult?.Value
                    )
                  );
        }
    }
}