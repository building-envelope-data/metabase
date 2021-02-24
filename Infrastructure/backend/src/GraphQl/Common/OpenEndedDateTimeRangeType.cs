using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using DateTime = System.DateTime;

namespace Infrastructure.GraphQl.Common
{
    public sealed class OpenEndedDateTimeRangeType
      : ObjectType<NpgsqlRange<DateTime>>
    {
        protected override void Configure(
            IObjectTypeDescriptor<NpgsqlRange<DateTime>> descriptor
            )
        {
            descriptor.BindFieldsExplicitly();

            descriptor.Name("OpenEndedDateTimeRange");

            descriptor
              .Field("from")
              .Type<DateTimeType>()
              .Resolver(context =>
                  {
                      var range = context.Parent<NpgsqlRange<DateTime>>();
                      return range.LowerBoundInfinite
                      ? null
                      : range.LowerBound;
                  }
                  );

            descriptor
              .Field("to")
              .Type<DateTimeType>()
              .Resolver(context =>
                  {
                      var range = context.Parent<NpgsqlRange<DateTime>>();
                      return range.UpperBoundInfinite
                      ? null
                      : range.UpperBound;
                  }
                  );
        }
    }
}