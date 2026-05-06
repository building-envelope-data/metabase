import { Typography } from "antd";
import dayjs from "dayjs";
import { OpenEndedDateTimeRange } from "../__generated__/graphql";
import { isTruthy } from "../lib/array";

interface OpenEndedDateTimeRangeProps {
  range: OpenEndedDateTimeRange;
}

export default function OpenEndedDateTimeRangeX({
  range,
}: OpenEndedDateTimeRangeProps) {
  return (
    <Typography.Text>
      {[
        range.from == null && range.to == null && "unrestricted",
        range.from != null && `from ${dayjs(range.from).format("DD/MM/YYYY")}`,
        range.to != null && `to ${dayjs(range.to).format("DD/MM/YYYY")}`,
      ]
        .filter(isTruthy)
        .join(" ")}
    </Typography.Text>
  );
}
