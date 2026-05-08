import { Typography } from "antd";
import { OpenEndedDateTimeRange } from "../__generated__/graphql";
import { intersperse, isTruthy } from "../lib/array";
import DateTimeX from "./DateTimeX";

interface OpenEndedDateTimeRangeProps {
  range: OpenEndedDateTimeRange;
}

export default function OpenEndedDateTimeRangeX({
  range,
}: OpenEndedDateTimeRangeProps) {
  return (
    <Typography.Text>
      {intersperse(
        [
          range.from == null && range.to == null && "unrestricted",
          range.from != null && (
            <>
              from <DateTimeX value={range.from} />
            </>
          ),
          range.to != null && (
            <>
              to <DateTimeX value={range.to} />
            </>
          ),
        ].filter(isTruthy),
        " ",
      )}
    </Typography.Text>
  );
}
