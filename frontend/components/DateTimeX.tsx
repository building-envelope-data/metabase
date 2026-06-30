import { Calendar, Popover } from "antd";
import dayjs from "dayjs";
import { Scalars } from "../__generated__/graphql";

type DateTimeProps =
  | {
      value: Scalars["DateTime"]["output"];
    }
  | {
      parsedValue: dayjs.Dayjs;
    };

export default function DateTimeX(props: DateTimeProps) {
  const parsedValue = "value" in props ? dayjs(props.value) : props.parsedValue;

  const calendarContent = (
    <div style={{ width: 300 }}>
      <Calendar
        value={parsedValue}
        fullscreen={false}
        headerRender={() => null}
      />
    </div>
  );

  return (
    <Popover
      title={false}
      content={calendarContent}
      trigger="hover"
      placement="bottomLeft"
    >
      <span style={{ cursor: "pointer", borderBottom: "1px solid grey" }}>
        {parsedValue.format("YYYY-MM-DD HH:mm")}
      </span>
    </Popover>
  );
}
