import { Typography } from "antd";
import dayjs from "dayjs";
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
      from {dayjs(range.from).format("DD/MM/YYYY") || "beginning of time"} to{" "}
      {dayjs(range.to).format("DD/MM/YYYY") || "end of time"}
    </Typography.Text>
  );
}
