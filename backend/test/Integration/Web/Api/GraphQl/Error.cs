using System.Collections.Generic;
using Icon;

namespace Test.Integration.Web.Api.GraphQl
{
    public class Error : ResponseBase
    {
        public string? message { get; set; }
        public IReadOnlyList<Location>? locations { get; set; }
        public IReadOnlyList<string>? path { get; set; } // TODO `path` is actually a list of strings or ints, see https://graphql.github.io/graphql-spec/June2018/#sec-Errors, how do we represent this in C# which sadly does not have union types as requested in https://github.com/dotnet/csharplang/issues/399
        public Extensions? extensions { get; set; }

        public class Location : ResponseBase
        {
            public int? line { get; set; }
            public int? column { get; set; }
        }

        public class Extensions : ResponseBase
        {
            public string? code { get; set; }
        }

        public override void EnsureNoOverflow()
        {
            base.EnsureNoOverflow();
            extensions.NotNull().EnsureNoOverflow();
            foreach (var location in locations.NotNull())
            {
                location.EnsureNoOverflow();
            }
        }
    }
}