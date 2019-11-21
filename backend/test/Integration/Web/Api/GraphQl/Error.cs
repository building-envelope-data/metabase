using Icon;
using System.Collections.Generic;

namespace Test.Integration.Web.Api.GraphQl
{
        public class Error : ResponseBase
        {
            public string? message { get; set; }
            public IReadOnlyList<Location>? locations { get; set; }
            public IReadOnlyList<string>? path { get; set; }
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
