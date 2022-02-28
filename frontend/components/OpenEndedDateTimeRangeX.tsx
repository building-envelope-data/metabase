import { Typography } from "antd";
import { OpenEndedDateTimeRange } from "../__generated__/__types__";

export type OpenEndedDateTimeRangeProps = {
  range: OpenEndedDateTimeRange | null | undefined;
};

export default function OpenEndedDateTimeRangeX({
  range,
}: OpenEndedDateTimeRangeProps) {
  return range == null ? (
    <Typography.Text>Unknown</Typography.Text>
  ) : (
    <Typography.Text>
      from {range.from || "beginning of time"} to {range.to || "end of time"}
    </Typography.Text>
  );
}
