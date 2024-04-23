using DateTime = System.DateTime;

namespace Metabase.GraphQl.Common
{
    public sealed record OpenEndedDateTimeRangeInput(
        DateTime? From,
        DateTime? To
    );
}