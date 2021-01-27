using DateTime = System.DateTime;

namespace Metabase.GraphQl.Common
{
    public record OpenEndedDateTimeRangeInput(
          DateTime? From,
          DateTime? To
        );
}