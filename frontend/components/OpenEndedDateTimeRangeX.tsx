import { Typography } from "antd";
import dayjs from "dayjs";
import { OpenEndedDateTimeRange } from "../__generated__/graphql";

interface OpenEndedDateTimeRangeProps {
  range: OpenEndedDateTimeRange;
}

export default function OpenEndedDateTimeRangeX({
  range,
}: OpenEndedDateTimeRangeProps) {
  return (
    <Typography.Text>
      from{" "}
      {range.from == null
        ? "beginning of time"
        : dayjs(range.from).format("DD/MM/YYYY")}{" "}
      to{" "}
      {range.to == null ? "end of time" : dayjs(range.to).format("DD/MM/YYYY")}
    </Typography.Text>
  );
}
