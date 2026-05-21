import { Calendar, Popover } from "antd";
import dayjs from "dayjs";

interface DateTimeProps {
  value: Parameters<typeof dayjs>[0];
}

export default function DateTimeX({ value }: DateTimeProps) {
  const parsedValue = dayjs(value);

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
